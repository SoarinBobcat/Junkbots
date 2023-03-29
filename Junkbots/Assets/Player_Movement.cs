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

    float k_forward;
    float k_backward;
    float k_left;
    float k_right;
    float x_move;
    float z_move;
    Vector3 dir = Vector3.zero;
    Vector3 currSpd = Vector3.zero;

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

    // Start is called before the first frame update
    void Start()
    {
        c_c = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get Player Input
        k_forward = Input.GetKey(KeyCode.W) ? 1 : 0;
        k_backward = Input.GetKey(KeyCode.S) ? 1 : 0;
        k_left = Input.GetKey(KeyCode.A) ? 1 : 0;
        k_right = Input.GetKey(KeyCode.D) ? 1 : 0;

        //Convert Input to Axes
        x_move = k_right-k_left;
        z_move = k_forward-k_backward;

        //Convert Axes to Direction Vector
        dir = new Vector3(x_move, 0, z_move);
        dir = Vector3.Normalize(dir);

        //Add acceleration to current velocity...or decceleration if no meowvement =|owo|=
        //Strafing
        if ((Mathf.Abs(x_move) == 1)) {
            if (c_c.isGrounded)
            {
                currSpd.x += dir.x * g_acc;
            } else
            {
                currSpd.x += dir.x * a_acc;
            }
        }
        else if (c_c.isGrounded)
        {
            if ((Mathf.Abs(currSpd.x) - Mathf.Sign(currSpd.x) * decc) > 0)
            {
                currSpd.x -= Mathf.Sign(currSpd.x) * decc;
            }
            else
            {
                currSpd.x = 0;
            }
        }

        //Forwards and Backwards
        if ((Mathf.Abs(z_move) == 1)) {
            if (c_c.isGrounded)
            {
                currSpd.z += dir.z * g_acc;
            }
            else
            {
                currSpd.z += dir.z * a_acc;
            }
        } else if (c_c.isGrounded)
        {
            if ((Mathf.Abs(currSpd.z) - Mathf.Sign(currSpd.z) * decc) > 0)
            {
                currSpd.z -= Mathf.Sign(currSpd.z) * decc;
            }
            else
            {
                currSpd.z = 0;
            }
        }

        //Jumping "Yahoo~♪"
        if (!c_c.isGrounded){
            currSpd.y -= grav;
        } else
        {
            currSpd.y = -0.1f;
        }

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
}
