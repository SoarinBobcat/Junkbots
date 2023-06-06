using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
	public int speed = 100;
	//int accel_air;


	public float verti, hori;

	public float gravity = 0.2f;	
	public int jump_power = 9;

	public Vector3 direction;
	public CharacterController c_c;
	
	private Animator robo_anim;
	
	void Start()
	{
		c_c = GetComponent<CharacterController>();
		robo_anim = GetComponentInChildren<Animator>();
		//accel_air = accel / 10;
	}


	void Update()
	{
		//mapping axis to controls
		verti = Input.GetAxis("Vertical");
		hori = Input.GetAxis("Horizontal");
				
		//adding gravity to our direction variable
		if (!c_c.isGrounded)
		{
			direction.y -= gravity;
        	}
		else
		{
			//direction.y = -1;

            		if (Input.GetButtonDown("Jump"))
            		{
                		direction.y = jump_power;
        		}


		}
		
		

		direction.z = verti * speed * Time.deltaTime;
		direction.x = hori * speed * Time.deltaTime;

		//c_c.Move(transform.forward * speed * verti * Time.deltaTime);
		//c_c.Move(transform.right * speed * hori * Time.deltaTime);

		c_c.Move(direction * Time.deltaTime);

		//if grounded use normal speed/accel, else use in air speed/accel
		//need access to deaccel too
		
		//animations
		if(direction.x == 0 && direction.z == 0)
		{
			//Idle
			robo_anim.SetFloat("speed", 0);
		}
		else
		{
			robo_anim.SetFloat("speed", 1);
		}
		
		//Debug.Log(direction);
		
	}
}
