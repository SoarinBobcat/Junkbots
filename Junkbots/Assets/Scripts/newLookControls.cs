using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newLookControls : MonoBehaviour
{
	private const float min_y = -40;
	private const float max_y = 40;
	
	private float mouse_x = 0f;
	private float mouse_y = 0f;

	private float sensitivity_x = 4f;
	private float sensitivity_y = 2f;
	
	private Camera cam;
	
    // Start is called before the first frame update
    void Start()
    {
		cam = Camera.main;
		
		
		
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouse_x += Input.GetAxis("Mouse X") * sensitivity_x;
		mouse_y -= Input.GetAxis("Mouse Y") * sensitivity_y;
		
		mouse_y = Mathf.Clamp(mouse_y, min_y, max_y);
		
		transform.Rotate(Vector3.up, mouse_x * sensitivity_x);
		transform.Rotate(Vector3.left, mouse_y * sensitivity_y);
    }
}
