﻿using UnityEngine;
using System.Collections;

using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core {

public class Pitcher : MonoBehaviour {

    [Inject]
    public VehicleReachedDestination onVehicleReachedDestination { get; set; }

    [Inject]
    public VehicleCrashed onVehicleCrashed { get; set; }

    [Inject]
    public LevelFailed onLevelFailed { get; set; }

    [Inject]
    public LevelComplete onLevelComplete { get; set; }

    [Inject]
    public ScoreGrow onScoreGrow { get; set; }

	public float Pause = 2;
    private float _pause;

	public float IntervalMin = 3;
	public float IntervalMax = 5;

	public Transform vehicle;

	private float spawnTime = 0;

	public Transform[] vehicles;

	// Use this for initialization
	void Start () {
        onLevelFailed.AddListener(stopSpawn);
        onLevelComplete.AddListener(stopSpawn);

        _pause = Pause;
	}

    void stopSpawn()
    {
        spawnTime = float.MaxValue;
    }

    void OnDestroy()
    {
        onLevelFailed.RemoveListener(stopSpawn);
        onLevelComplete.RemoveListener(stopSpawn);
    }

	// Update is called once per frame
	void Update () {
		if (_pause > 0) {
			_pause -= Time.deltaTime;
			return;
		}
		spawnTime -= Time.deltaTime;;
		if (spawnTime < 0) {
			spawnTime = Random.value * (IntervalMax-IntervalMin) + IntervalMin;		
            Transform t = (vehicles.Length==0) ? vehicle : vehicles[Random.Range(0,vehicles.Length)];
            Transform v  = Instantiate(t, this.transform.localPosition, this.transform.localRotation) as Transform;
            Vehicle v2 = v.GetComponent<Vehicle>();

            v.parent = this.gameObject.transform.parent;

            v2.onVehicleCrashed = onVehicleCrashed;
            v2.onVehicleReachedDestination = onVehicleReachedDestination;
            v2.onLevelComplete = onLevelComplete;
            v2.onScoreGrow = onScoreGrow;
		}

	}

    public void Reset()
    {
        _pause = Pause;
        spawnTime = 0;
    }
}

}