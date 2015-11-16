using UnityEngine;
using System.Collections;

public class Pitcher : MonoBehaviour {

	public float Pause = 2;

	public float IntervalMin = 3;
	public float IntervalMax = 5;

	public Transform vehicle;

	private float spawnTime = 0;
	private Level level = null;

	public Transform[] vehicles;

	// Use this for initialization
	void Start () {
		if (GameObject.Find ("Level")!=null)
			level = GameObject.Find ("Level").GetComponent<Level>();
	}

	// Update is called once per frame
	void Update () {
		if (level!=null && (level.Crash || level.Complete || level.PreStart))
			return;

		if (Pause > 0) {
			Pause -= Time.deltaTime;
			return;
		}
		spawnTime -= Time.deltaTime;;
		if (spawnTime < 0) {
			spawnTime = Random.value * (IntervalMax-IntervalMin) + IntervalMin;
			Transform v;
			if (vehicles.Length==0)
				v = Instantiate(vehicle, gameObject.transform.position, gameObject.transform.localRotation) as Transform;
			else
				v = Instantiate(vehicles[Random.Range(0,vehicles.Length)], gameObject.transform.position, gameObject.transform.localRotation) as Transform;

			//v.localScale = new Vector3(0.5f,0.5f,0.5f);
			//v.gameObject.tag = "VehicleAI";
		}

	}
}
