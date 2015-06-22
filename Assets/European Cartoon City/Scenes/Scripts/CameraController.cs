using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public float speedX = 5;
	public float speedY = 5;
	public Vector3 standardRotation = new Vector3(45, 45, 0);
	
	void Start () 
	{
		transform.eulerAngles = standardRotation;
	}

	void LateUpdate () 
	{
		MoveCamera();
	}
	
	void MoveCamera()
	{
		// set camera heiht
		float height = transform.position.y;
		
		// move camera in local coordinates
		transform.Translate(new Vector3(speedX * Input.GetAxis("Horizontal"), 0, speedY * Input.GetAxis("Vertical")), Space.Self);
		
		// clamp camera to height
		transform.position = new Vector3(transform.position.x, height, transform.position.z);
	}
}
