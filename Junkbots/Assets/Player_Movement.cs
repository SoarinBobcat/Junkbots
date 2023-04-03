using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //Player Speed Variables
    int spd = 8;
    float g_acc = 0.3f;
    float a_acc = 0.05f;
    float decc = 0.2f;

    float x_move;
    float z_move;
    Vector3 xMove;
    Vector3 zMove;
    Vector3 dir = Vector3.zero;
    Vector3 currSpd = Vector3.zero;

    //Turning Variables
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float targetAngle = 0;

    //Player Jump Variables
    float grav = 0.2f;
    int jumpSpd = 20;

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
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

        x_move = Input.GetAxisRaw("Horizontal");
        z_move = Input.GetAxisRaw("Vertical");

        xMove = x_move * cam.right;
        zMove = z_move * cam.forward;

        dir = xMove + zMove;

        //Turn to face movement dir
        if ((x_move != 0) || (z_move != 0))
        {
            targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        }
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //Add acceleration to current velocity...or decceleration if no meowvement =|owo|=
        if ((x_move != 0) || (z_move != 0))
        {
            if (c_c.isGrounded)
            {
                currSpd += xMove * g_acc + zMove * g_acc;
            }
            else
            {
                currSpd += xMove * a_acc + zMove * a_acc;
            }
        } 
        
        if (c_c.isGrounded)
        {
            currSpd.x = deccelerate(currSpd.x);
            currSpd.z = deccelerate(currSpd.z);
        }

        //Clamp speed
        currSpd.x = Mathf.Clamp(currSpd.x, -spd, spd);
        currSpd.z = Mathf.Clamp(currSpd.z, -spd, spd);

        //Jumping "Yahoo~♪"
        //Apply gravity when in air
        if (!c_c.isGrounded){
            currSpd.y -= grav;
        } 
        else
        {
            currSpd.y = -1f;
        }

        //Jump if grounded
        if (Input.GetKeyDown(KeyCode.Space) && c_c.isGrounded)
        {
            currSpd.y += jumpSpd;
        }

        //Apply velocity
        c_c.Move(currSpd * Time.deltaTime);
    }

    /*
    Calculates the acceleration and decceleration of an axis;
    d = dir.[The axis to be calculated]
    vel = currSpd.[The axis to be calculated]*/
    
    private float deccelerate(float vel)
    {
      
        if ((Mathf.Abs(vel) - Mathf.Sign(vel) * decc) > 0)
        {
            vel -= Mathf.Sign(vel) * decc;
        }
        else
        {
            vel = 0;
        }

        return vel;
    }
}
