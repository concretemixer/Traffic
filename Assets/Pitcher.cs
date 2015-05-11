using UnityEngine;
using System.Collections;

public class Pitcher : MonoBehaviour {

	public float Pause = 2;

	public float IntervalMin = 3;
	public float IntervalMax = 5;

	public Transform vehicle;

	private float spawnTime = 0;
	private Level level;

	// Use this for initialization
	void Start () {
		level = GameObject.Find ("Level").GetComponent<Level>();
	}

	// Update is called once per frame
	void Update () {
		if (level.Crash || level.Complete)
			return;

		if (Pause > 0) {
			Pause -= Time.deltaTime;
			return;
		}
		spawnTime -= Time.deltaTime;;
		if (spawnTime < 0) {
			Instantiate(vehicle, gameObject.transform.position, gameObject.transform.localRotation);
			spawnTime = Random.value * (IntervalMax-IntervalMin) + IntervalMin;
		}

	}
}
