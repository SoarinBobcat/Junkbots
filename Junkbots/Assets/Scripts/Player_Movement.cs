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
	//bool jumpQueue = false;


    //Current Velocity Variables
    float currSpd = 0f;
    Vector3 vel = Vector3.zero;
    float accel = 0;

    //Turning Variables
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float targetAngle = 0;

    //Attack Variables
    float attackCooldown = 1f;
    bool canAttack = true;
    float range = 1f;

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
    public GameObject hitbox;
    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        c_c = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Get Player Input
        xMove = Input.GetAxisRaw("Horizontal") * cam.right;
        zMove = Input.GetAxisRaw("Vertical") * cam.forward;

        /*if (Input.GetButton("Fire1")) {
            s = (int) STATES.Attack;
        }*/

        dir = xMove + zMove;
        dir.y = 0;
        dir.Normalize();

        //Turn to face movement dir
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

        //State Machine
        switch (s)
        {
            case 0:
                if (c_c.isGrounded)
                {
                    GroundMove();
                }
                else
                {
                    AirMove();
                }
                break;
            case 1:
                Attack();
                break;
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
        vel.y = -grav;
		
		if (Input.GetKey(KeyCode.Space))
		{
			vel.y = jumpSpd;
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

    //Handles Attacks
    private void Attack()
    {
        
    }
}