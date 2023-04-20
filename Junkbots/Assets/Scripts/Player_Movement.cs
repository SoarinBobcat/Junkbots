using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
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

    //Player Speed Variables
    [Header("Movement")]
    [SerializeField] private MovementSettings gSettings = new MovementSettings(20, 1, 0.5f);
    [SerializeField] private MovementSettings aSettings = new MovementSettings(20, 0.5f, 0.5f);
    [SerializeField] private float fric = 0.1f;

    //Player Jump Variables
    [SerializeField] private float grav = 1f;
    [SerializeField] private float jumpSpd = 20f;

    //Input Variables
    Vector3 xMove;
    Vector3 zMove;
    Vector3 dir = Vector3.zero;

    //Current Velocity Variables
    float currSpd = 0f;
    Vector3 vel = Vector3.zero;
    float accel = 0;

    //Turning Variables
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float targetAngle = 0;

    //State Machine stuff for later ;)
    enum STATES
    {
        Main,
        Attack,
        AltAttack,
        Dodge,
        Hurt
    }

    int s = (int) STATES.Main;

    private CharacterController c_c;
    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        c_c = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get Player Input
        xMove = Input.GetAxisRaw("Horizontal") * cam.right;
        zMove = Input.GetAxisRaw("Vertical") * cam.forward;

        dir = xMove + zMove;
        dir.y = 0;
        dir.Normalize();

        //Turn to face movement dir
        if ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0))
        {
            targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        }
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //Movement
        if (c_c.isGrounded)
        {
            GroundMove();
        }
        else
        {
            AirMove();
        }

        //Apply velocity
        c_c.Move(vel * Time.deltaTime);
    }

    //Handles Ground Movement
    private void GroundMove()
    {
        Friction();

        //Check to see if the player is trying to move opposite to their current velocity
        if (Vector3.Dot(vel, dir) < 0)
        {
            accel = gSettings.Decc;
        }
        else
        {
            accel = gSettings.Acc;
        }

        //Jumping "Yahoo~♪"
        vel.y = -1f;

        //Jump if grounded
        if (Input.GetKeyDown(KeyCode.Space) && c_c.isGrounded)
        {
            vel.y += jumpSpd;
        }

        //Apply Accelearation and Clamp Speed
        vel.x = Mathf.Clamp((vel.x + (dir.x * accel)), -gSettings.Spd, gSettings.Spd);
        vel.z = Mathf.Clamp((vel.z + (dir.z * accel)), -gSettings.Spd, gSettings.Spd);
    }

    //Applies Friction
    private void Friction()
    {
        Vector3 velocity = vel;
        velocity.y = 0;
        float speed = velocity.magnitude;
        float drop = 0;

        float control = speed < gSettings.Decc ? gSettings.Decc : speed;
        drop = control * fric;

        float newSpeed = speed - drop;
        if (newSpeed < 0)
        {
            newSpeed = 0;
        }

        if (newSpeed > 0)
        {
            newSpeed /= speed;
        }

        vel.x *= newSpeed;
        vel.z *= newSpeed;
    }

    //Handles Air Movement
    private void AirMove()
    {
        //Apply Gravity
        vel.y -= grav;

        //Apply Accelearation and Clamp Speed
        accel = aSettings.Acc;

        vel.x = Mathf.Clamp((vel.x + (dir.x * accel)), -gSettings.Spd, gSettings.Spd);
        vel.z = Mathf.Clamp((vel.z + (dir.z * accel)), -gSettings.Spd, gSettings.Spd);
    }
}