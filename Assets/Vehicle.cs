using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour {

	private Level level;
	// Use this for initialization
	void Start () {

		GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (10, 0, 0);
		level = GameObject.Find ("Level").GetComponent<Level>();
	}
	
	// Update is called once per frame
	void Update () {
		//GetComponent<Rigidbody> ().velocity = new Vector3 (50, 0, 0);
		//GetComponent<Rigidbody> ().AddForce(new Vector3 (50, 0, 0));
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "Finish")
		{
			level.OnReach(this);
			Destroy(gameObject);
		}
		if (col.gameObject.tag == "Vehicle") 
		{
			GetComponent<Rigidbody> ().drag = 10;
			GetComponent<Rigidbody> ().angularDrag = 10;

			level.OnCrash(); 
		}
	}
}
