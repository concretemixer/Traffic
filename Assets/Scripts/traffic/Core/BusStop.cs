using UnityEngine;
using System.Collections;

namespace Traffic.Core {

public class BusStop : MonoBehaviour {
	
	float StopTimeMin = 2000;
	float StopTimeMax = 3500;

	public int GetStopTime()
	{
		return (int)(Random.value * (StopTimeMax - StopTimeMin) + StopTimeMin);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}

}