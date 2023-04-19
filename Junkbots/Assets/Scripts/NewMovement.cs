using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NewMovement : MonoBehaviour
{
    //Movment Variable Class Set Up
    [System.Serializable]
    public class MovementSettings
    {
        public float Spd;
        public float Acc;
        public float Decc;

        public MovementSettings(float spd, float acc, float decc)
        {
            Spd = spd;
            Acc = acc;
            Decc = decc;
        }
    }

    //Movment Variables
    [Header("Movement")]
    [SerializeField] private float fric = 6;
    [SerializeField] private float grav = 20;
    [SerializeField] private float jumpSpd = 8;

    [Tooltip("Auto jump on holding button")]
    [SerializeField] private bool autoHop = false;

    [Tooltip("Air Control Precision")]
    [SerializeField] private float airControl = 0.3f;

    //Acceleraction, decceleration and max speed of the player in different states
    [SerializeField] private MovementSettings groundSettings = new MovementSettings(7, 14, 10);
    [SerializeField] private MovementSettings airSettings = new MovementSettings(7, 2, 2);
    [SerializeField] private MovementSettings strafeSettings = new MovementSettings(1, 50, 50);

    public float speed { get { return c_c.velocity.magnitude; } }

    //Init player components as well as input and velocity variables
    private CharacterController c_c;
    private Camera camera;
    private Vector3 moveDirNorm = Vector3.zero;
    private Vector3 playerVel = Vector3.zero;

    private bool queueJump = false;

    private Vector3 moveInput;
    private Transform trans;
    private Transform camTrans;

    private void Start()
    {
        trans = transform;
        c_c = GetComponent<CharacterController>();

        if (!camera)
        {
            camera = Camera.main;
        }

        camTrans = camera.transform;
    }

    private void Update()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        QueueJump();

        if (c_c.isGrounded)
        {
            GroundMove();
        }
        else
        {
            AirMove();
        }

        //Rotate player and cam code

        //Move the character
        c_c.Move(playerVel * Time.deltaTime);
    }

    //Buffers jump if jump is held before hitting the ground
    private void QueueJump()
    {
        if (autoHop) {
            queueJump = Input.GetButton("Jump");
            return;
        }

        if (Input.GetButtonDown("Jump") && !queueJump)
        {
            queueJump = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            queueJump = false;
        }
    }

    //Handle Air Movement
    private void AirMove()
    {
        float accel;

        var wishDir = new Vector3(moveInput.x, 0, moveInput.z);
        wishDir = trans.TransformDirection(wishDir);

        float wishSpd = wishDir.magnitude;
        wishSpd *= airSettings.Spd;

        wishDir.Normalize();
        moveDirNorm = wishDir;

        //Check to see if the player is trying to move opposite to their current velocity
        float wishSpd2 = wishSpd;
        if (Vector3.Dot(playerVel, wishDir) < 0)
        {
            //If they are use decceleration instead
            accel = airSettings.Decc;
        }
        else
        {
            //...Otherwise accelerate as usual
            accel = airSettings.Acc;
        }

        if (moveInput.z == 0 && moveInput.x != 0)
        {
            if (wishSpd > strafeSettings.Spd)
            {
                wishSpd = strafeSettings.Spd;
            }

            accel = strafeSettings.Acc;
        }

        Accelerate(wishDir, wishSpd, accel);
        if (airControl > 0)
        {
            AirControl(wishDir, wishSpd2);
        }

        playerVel.y -= grav * Time.deltaTime;
    }

    //Handle Air Stafing
    private void AirControl(Vector3 targetDir, float targetSpd)
    {
        // Only control air movement when moving forward or backward.
        if (Mathf.Abs(moveInput.z) < 0.001 || Mathf.Abs(targetSpd) < 0.001)
        {
            return;
        }

        //Store and remove y velocity
        float ySpeed = playerVel.y;
        playerVel.y = 0;
        //Get player lateral speed then convert speed in to direction
        float speed = playerVel.magnitude;
        playerVel.Normalize();

        float dot = Vector3.Dot(playerVel, targetDir);
        float k = 32;
        k *= airControl * dot * dot * Time.deltaTime;

        if (dot > 0)
        {
            playerVel.x *= speed + targetDir.x * k;
            playerVel.y *= speed + targetDir.y * k;
            playerVel.z *= speed + targetDir.z * k;

            playerVel.Normalize();
            moveDirNorm = playerVel;
        }

        playerVel.x *= speed;
        playerVel.y = ySpeed;
        playerVel.z *= speed;
    }

    //Handle Ground Movement
    private void GroundMove()
    {
        //Apply friction if jump is not queued
        if (!queueJump)
        {
            ApplyFriction(1.0f);
        }

        var wishDir = new Vector3(moveInput.x, 0, moveInput.z);
        wishDir = trans.TransformDirection(wishDir);
        wishDir.Normalize();
        moveDirNorm = wishDir;

        var wishSpd = wishDir.magnitude;
        wishSpd *= groundSettings.Spd;

        Accelerate(wishDir, wishSpd, groundSettings.Acc);

        playerVel.y = -grav * Time.deltaTime;

        if (queueJump)
        {
            playerVel.y = jumpSpd;
            queueJump = false;
        }
    }

    //As the name suggests this function applies friction. 't' is the percentage of which the friction is applied; 1.0f = 100%, 0 = 0%.
    private void ApplyFriction(float t)
    {
        Vector3 vel = playerVel;
        vel.y = 0;
        float speed = vel.magnitude;
        float drop = 0;

        float control = speed < groundSettings.Decc ? groundSettings.Decc : speed;
        drop = control * fric * Time.deltaTime * t;

        float newSpeed = speed - drop;
        if (newSpeed < 0)
        {
            newSpeed = 0;
        }

        if (newSpeed > 0)
        {
            newSpeed /= speed;
        }

        playerVel.x *= newSpeed;
        playerVel.z *= newSpeed;
    }

    private void Accelerate(Vector3 targetDir, float targetSpd, float accel)
    {
        float currentSpd = Vector3.Dot(playerVel, targetDir);
        float addSpd = targetSpd - currentSpd;
        if (addSpd <= 0)
        {
            return;
        }

        float accelSpd = accel * Time.deltaTime * targetSpd;
        if (accelSpd > addSpd)
        {
            accelSpd = addSpd;
        }

        playerVel.x += accelSpd * targetDir.x;
        playerVel.z += accelSpd * targetDir.y;
    }
}