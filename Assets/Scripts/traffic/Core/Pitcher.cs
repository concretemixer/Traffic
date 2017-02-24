using UnityEngine;
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

    private bool ready = false;

	public float Pause = 2;
    private float _pause;

	public float IntervalMin = 3;
	public float IntervalMax = 5;

	private float spawnTime = 0;

	public Transform[] vehicles;

    public bool Sequental = false;
    private int nextVehicleIdx = 0;

	// Use this for initialization
	public void OnReady ()
    {
        ready = true;

        if (onLevelFailed != null)
            onLevelFailed.AddListener(stopSpawn);
        if (onLevelComplete != null)
            onLevelComplete.AddListener(stopSpawn);

        _pause = Pause;
	}

    void stopSpawn()
    {
        spawnTime = float.MaxValue;
    }

    void OnDestroy()
    {
        if (onLevelFailed != null)
            onLevelFailed.RemoveListener(stopSpawn);
        if (onLevelComplete != null)
            onLevelComplete.RemoveListener(stopSpawn);
    }

	// Update is called once per frame
	void Update () {
        if (!ready)
            return;

		if (_pause > 0) {
			_pause -= Time.deltaTime;
			return;
		}
		spawnTime -= Time.deltaTime;;
		if (spawnTime < 0) {
			spawnTime = Random.value * (IntervalMax-IntervalMin) + IntervalMin;		
            Transform t = Sequental ? vehicles[nextVehicleIdx] : vehicles[Random.Range(0,vehicles.Length)];
            nextVehicleIdx = (nextVehicleIdx + 1) % vehicles.Length;
            Transform v  = Instantiate(t, this.transform.localPosition, this.transform.localRotation) as Transform;
            Vehicle v2 = v.GetComponent<Vehicle>();
            v2.Number = Vehicle.NextNumber;
            Vehicle.NextNumber++;
            v.parent = this.gameObject.transform.parent;

            v2.onVehicleCrashed = onVehicleCrashed;
            v2.onVehicleReachedDestination = onVehicleReachedDestination;
            v2.onLevelComplete = onLevelComplete;
            v2.onLevelFailed = onLevelFailed;
            v2.onScoreGrow = onScoreGrow;
		}

	}

    public void Reset()
    {
        _pause = Pause;
        spawnTime = 0;
        nextVehicleIdx = 0;
        Vehicle.NextNumber = 1;
    }
}

}