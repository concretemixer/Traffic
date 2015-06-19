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
		} else if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
			position = Input.mousePosition;
		} else
			return;

		if (position != null) 
		{
			Ray ray = Camera.main.ScreenPointToRay( position );
			RaycastHit hit;
			
			if ( Physics.Raycast(ray, out hit)) {
				//Debug.Log(hit.transform.gameObject.name);
				if (hit.transform.gameObject.tag == "Vehicle")
				{
					Vehicle vehicle = hit.transform.GetComponent<Vehicle>();
					if (Input.GetMouseButtonDown (1))
						vehicle.SlowDown();
					if (Input.GetMouseButtonDown (0))
						vehicle.SpeedUp();
				}
			}

		}	
	}
}
