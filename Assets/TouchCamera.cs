using UnityEngine;
using System.Collections;

public class TouchCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 position;

		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
			position = Input.GetTouch (0).position; 
		} else if (Input.GetMouseButtonDown (0)) {
			position = Input.mousePosition;
		} else
			return;

		if (position != null) 
		{
			Ray ray = Camera.main.ScreenPointToRay( position );
			RaycastHit hit;
			
			if ( Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "Vehicle")
			{
				hit.rigidbody.velocity = new Vector3(0,0,0);
			}
		}	
	}
}
