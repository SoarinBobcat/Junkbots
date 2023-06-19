﻿//Main Player Script
//Riley Stephens Last Updated: 19/06/2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Main : MonoBehaviour
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
    [SerializeField] private float grav = 0.8f;
    [SerializeField] private float jumpSpd = 20f;

    //Input Variables
    Vector3 xMove;
    Vector3 zMove;
    Vector3 dir = Vector3.zero;
    //bool jumpQueue = false;


    //Current Velocity Variables
    float currSpd = 0f;
    Vector3 vel = Vector3.zero;
    Vector3 vel2 = Vector3.zero;
    float accel = 0;

    //Turning Variables
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float targetAngle = 0;

    //Attack Variables
    float attackStart = 0.02f;
    float attackActive = 0.25f;
    float attackCooldown = 0.4f;
    float attackTimer = 0f;
    float range = 1f;
    [SerializeField] public float damage = 5f;

    //State Machine stuff for later ;)
    enum STATES
    {
        Main,
        Attack,
        AltAttack,
        Dodge,
        Hurt
    }

    int s = (int)STATES.Main;

    //Attack States
    enum ATTACK
    {
        //Not Active
        Idle,
        //Attack In Progress but no active hitbox
        StartUp,
        //Active hitbox
        Active,
        //Attack is ending usually no hitbox
        Cooldown
    }

    int aS = (int)ATTACK.Idle;

    //Durability States
    enum DURABILITY
    {
        Perfect,
        Damaged,
        Critical,
        Missing
    }

    //Body Initialisation
    Dictionary<string, int> partDurability = new Dictionary<string, int>()
    {
        {"Head", (int)DURABILITY.Damaged},
        {"Torso", (int)DURABILITY.Perfect},
        {"Arm_L", (int)DURABILITY.Missing},
        {"Arm_R", (int)DURABILITY.Missing},
        {"Leg_L", (int)DURABILITY.Perfect},
        {"Leg_R", (int)DURABILITY.Perfect}
    };

    //UI Textures
    //Grab UI
    public Animator Head;
    public Animator Torso;
    public Animator Arm_L;
    public Animator Arm_R;
    public Animator Leg_L;
    public Animator Leg_R;

    //Battery
    int battery = 100;
    int batteryMax = 100;

    //Grab Objects
    private CharacterController c_c;
    public GameObject hitbox;
    public Transform cam;
	public GameObject trail;

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

        if ((Input.GetButton("Fire1")) && (aS == (int)ATTACK.Idle)) {
            s = (int)STATES.Attack;
            aS = (int)ATTACK.StartUp;
            attackTimer = attackStart;
			trail.SetActive(true);
        }

        dir = xMove + zMove;
        dir.y = 0;
        dir.Normalize();

        //Apply Gravity
        if (c_c.isGrounded)
        {
            vel.y = -grav;
        }
        else
        {
            vel.y -= grav;
        }

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
	
	private void LateUpdate()
	{
		//Turn to face movement dir
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
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
        //Apply Accelearation and Clamp Speed
        accel = aSettings.Acc;

        vel.x = Mathf.Clamp((vel.x + (dir.x * accel)), -gSettings.Spd, gSettings.Spd);
        vel.z = Mathf.Clamp((vel.z + (dir.z * accel)), -gSettings.Spd, gSettings.Spd);
    }

    //Handles Attacks
    private void Attack()
    {
        switch (aS)
        {
            case 1:
                vel.x *= 0.95f;
                vel.z *= 0.95f;
                break;
            case 2:
                vel2 = cam.forward * gSettings.Spd * 1.5f;
                vel2.y = vel.y;
                vel = vel2;
                break;
            case 3:
                vel.x *= 0.9f;
                vel.z *= 0.9f;
                break;

        }

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {	
            switch (aS)
            {
                case 0:
                    s = (int)STATES.Main;
					trail.SetActive(false);
                    break;
                case 1:
                    aS = (int)ATTACK.Active;
                    hitbox.SetActive(true);
                    attackTimer = attackActive;
                    break;
                case 2:
                    aS = (int)ATTACK.Cooldown;
                    hitbox.SetActive(false);
                    attackTimer = attackCooldown;
                    break;
                case 3:
                    aS = (int)ATTACK.Idle;
                    break;
            }
        }
    }
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			if (aS == (int)ATTACK.Active)
			{
				aS = (int)ATTACK.Idle;
			}
		}
	}

    /*GUI updates
    private void GUIUpdate()
    {
        Head.Play("Base Layer.head-damaged");
    }*/
}