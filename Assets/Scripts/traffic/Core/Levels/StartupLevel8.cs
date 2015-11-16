using UnityEngine;
using System.Collections;

public class StartupLevel8 : MonoBehaviour {

	double smokeTimer = 0;

	// Use this for initialization
	void Start () {
		GameObject.Find("SimplePeople_RoadWorker_White").GetComponent<Animator>().Play("Idle_SittingOnGround");

		smokeTimer = Random.value * 4 + 1.5;
	}
	
	// Update is called once per frame
	void Update () {
		if (smokeTimer > 0) {
			smokeTimer -= Time.deltaTime;
			if (smokeTimer <= 0) {
				//GameObject.Find("SimplePeople_StreetMan_Brown").GetComponent<Animator>().Play("Death_01");
				GameObject.Find ("SimplePeople_RoadWorker_Brown").GetComponent<Animator> ().Play ("Idle_Smoking");
			}		
		} else {
			//GameObject.Find("SimplePeople_StreetMan_Brown").GetComponent<Animator>().Play("Death_01");
			smokeTimer = Random.value * 4 + 1.5;
		}
	}
}
