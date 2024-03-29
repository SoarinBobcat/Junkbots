using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
	//Clamp values for looking up and down
	private const float Y_Min = -40;
	private const float Y_Max = 40;

	//Player and camera transforms
	public Transform lookAt;
	public Transform camTransform;

	private Camera cam;

	//Camera location variables
	private float distance = 4.0f;
	private Vector3 offset = new Vector3(0, 2, 0);
	Vector2 offYMinMax = new Vector2(0.25f, 2);

	public Vector3 dir = Vector3.zero;
	public Quaternion rot = new Quaternion(0,0,0,0);

	private float currX = 0f;
	private float currY = 0f;

	private float sensX = 4f;
	private float sensY = 2f;

	//Camera Collision
	Vector3 camDir;
	Vector2 camMinMax = new Vector2(1, 2);

	void Start()
    {
		camTransform = transform;
		cam = Camera.main;

		camDir = cam.transform.localPosition.normalized;

		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
    {
		//Rotate Camera
		currX += Input.GetAxis("Mouse X")*sensX;
		currY -= Input.GetAxis("Mouse Y")*sensY;

		//Lock camera y-axis
		currY = Mathf.Clamp(currY, Y_Min, Y_Max);
	}

	void LateUpdate()
	{
		CamCollision();
		Vector3 dir = new Vector3(0, 0, -distance);
		Quaternion rot = Quaternion.Euler(currY, currX, 0);

		//Lower camera offset when coming closer to player
		offset.y = Mathf.Clamp(offYMinMax.y * (distance / camMinMax.y), offYMinMax.x, offYMinMax.y);

		//Set position and rotation
		camTransform.position = (lookAt.position + offset) + rot * dir;

		camTransform.LookAt(lookAt.position);
		if (currY < 0)
		{
			camTransform.Rotate(-25 * (Mathf.Abs(currY) / Mathf.Abs(Y_Min)), 0, 0);
		}
	}


	//Check to see if camera is inside object
	void CamCollision()
    {
		distance = camMinMax.y;
		RaycastHit hit;
		if (Physics.Raycast(lookAt.position, ((camTransform.position-offset)-lookAt.position).normalized, out hit, camMinMax.y))
        {
			distance = Mathf.Clamp(hit.distance, camMinMax.x, camMinMax.y);
        }
    }
}
