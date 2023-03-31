using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
	public enum ThirdPersonCameraType {
		Track,
		Follow
	}
	
	public ThirdPersonCameraType mThirdPersonCameraType = ThirdPersonCameraType.Track;
	public Transform Player;
	public float PlayerHeight = 2.0f;
	public Vector3 targetPos;
	
    // Update is called once per frame
    void LateUpdate()
    {
        switch (mThirdPersonCameraType) {
			case ThirdPersonCameraType.Track: 
			{
				CameraMove_Track();
				break;
			}
			case ThirdPersonCameraType.Follow: 
			{
				CameraMove_Follow();
				break;
			}
		}
    }
	
	void CameraMove_Track() {
		targetPos = Player.transform.position;
		targetPos.y += PlayerHeight;
		transform.LookAt(targetPos);
	}
	
	void CameraMove_Follow() {
		Vector3 forward = transform.rotation * Vector3.forward;
		Vector3 right = transform.rotation * Vector3.right;
		Vector3 up = transform.rotation * Vector3.up;
		
		targetPos = Player.transform.position;
	}
}
