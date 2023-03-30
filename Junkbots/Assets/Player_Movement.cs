using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //Player Speed Variables
    int spd = 10;
    float g_acc = 0.2f;
    float a_acc = 0.05f;
    float decc = 0.3f;

    float x_move;
    float z_move;
    Vector3 dir = Vector3.zero;
    Vector3 currSpd = Vector3.zero;

    //Turning Variables
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float targetAngle = 0;

    //Player Jump Variables
    float grav = 0.3f;
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
        x_move = Input.GetAxisRaw("Horizontal");
        z_move = Input.GetAxisRaw("Vertical");

        //Turn to face movement dir
        if ((x_move != 0) || (z_move != 0))
        {
            targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        }
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //Convert Axes to Direction Vector
        dir = new Vector3(x_move, 0, z_move).normalized;

        //Add acceleration to current velocity...or decceleration if no meowvement =|owo|=
        currSpd.x = accNDecc(x_move, dir.x, currSpd.x);
        currSpd.z = accNDecc(z_move, dir.z, currSpd.z);

        //Jumping "Yahoo~♪"
        //Apply gravity when in air
        if (!c_c.isGrounded){
            currSpd.y -= grav;
        } else
        {
            currSpd.y = -1f;
        }

        //Jump if grounded
        if (Input.GetKeyDown(KeyCode.Space) && c_c.isGrounded)
        {
            currSpd.y += jumpSpd;
        }

        //Clamp speed
        currSpd.x = Mathf.Clamp(currSpd.x, -spd, spd);
        currSpd.z = Mathf.Clamp(currSpd.z, -spd, spd);

        //Apply velocity
        c_c.Move(currSpd * Time.deltaTime);
    }

    /*
    Calculates the acceleration and decceleration of an axis;
    axis = The variable holding current axis input (e.g x_move)
    d = dir.[The axis to be calculated]
    vel = currSpd.[The axis to be calculated]
    */
    private float accNDecc(float axis, float d, float vel)
    {
        if ((Mathf.Abs(axis) == 1))
        {
            if (c_c.isGrounded)
            {
                vel += d * g_acc;
            }
            else
            {
                vel += d * a_acc;
            }
        }
        else if (c_c.isGrounded)
        {
            if ((Mathf.Abs(vel) - Mathf.Sign(vel) * decc) > 0)
            {
                vel -= Mathf.Sign(vel) * decc;
            }
            else
            {
                vel = 0;
            }
        }

        return vel;
    }
}
