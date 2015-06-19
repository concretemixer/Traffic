using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour {

	public float NormalSpeed = 30;
	public float FastSpeed = 60;
	public bool  CanAccelerate = true;
	public bool IsBus = false;

	private Level level;

	private int gear = 1;
	private float stopTimer = 0;



	// Use this for initialization
	void Start () {

		GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (NormalSpeed, 0, 0);
		level = GameObject.Find ("Level").GetComponent<Level>();

		GetComponent<Renderer> ().material.color = new Color (Random.value, (float)(Random.value*0.2f), Random.value);
	}
	
	// Update is called once per frame
	void Update () {
		//GetComponent<Rigidbody> ().velocity = new Vector3 (50, 0, 0);
		//GetComponent<Rigidbody> ().AddForce(new Vector3 (50, 0, 0));
		if (stopTimer > 0) {
			stopTimer -= Time.deltaTime;
			if (stopTimer<=0)
				gear = 0;
				SpeedUp();
		}
	}


	void OnTriggerEnter(Collider col)
	{ 
		if(col.gameObject.tag == "Stop")
		{
			if (IsBus) {
				int pause = col.GetComponent<BusStop>().StopTimeMs;
				stopTimer =  (float)(pause / 1000.0);
				SlowDown();
				gear = -1;
				IsBus = false;
			}
		}
		if(col.gameObject.tag == "Finish")
		{
			level.OnReach(this);
			Destroy(gameObject);
		}
	}


	void OnCollisionEnter (Collision col)
	{

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
			GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (NormalSpeed, 0, 0);
		else
			return;

		gear--;


	}

	public void SpeedUp()
	{
		if (gear == 0)
			GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (NormalSpeed, 0, 0);
		else if (gear == 1) {
			if (CanAccelerate)
				GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (FastSpeed, 0, 0);
			else
				return;
		}
		else
			return;
		
		gear++;
	}

}
