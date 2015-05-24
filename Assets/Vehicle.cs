using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour {

	private Level level;

	private int gear = 1;
	// Use this for initialization
	void Start () {

		GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (20, 0, 0);
		level = GameObject.Find ("Level").GetComponent<Level>();

		GetComponent<Renderer> ().material.color = new Color (Random.value, Random.value*0.2, Random.value);
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

	public void SlowDown()
	{
		if (gear == 1)
			GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		else if (gear == 2)
			GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (20, 0, 0);
		else
			return;

		gear--;


	}

	public void SpeedUp()
	{
		if (gear == 0)
			GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (20, 0, 0);
		else if (gear == 1)
			GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (40, 0, 0);
		else
			return;
		
		gear++;
	}

}
