using UnityEngine;
using System.Collections;

namespace Traffic {

public class StartupLevel18 : MonoBehaviour {

	double moveTimer = 0;
	double moveTimer2 = 0;
	double moveTimer3 = 0;

	// Use this for initialization
	void Start () {
		moveTimer3 = Random.value * 4 + 10;
		moveTimer = Random.value * 4 + 1.5;
		GameObject.Find ("SimplePeople_BusinessMan_White").GetComponent<Animator> ().Play ("Smoking");
	}
	
	// Update is called once per frame
	void Update () {


		if (moveTimer > 0) {
			moveTimer -= Time.deltaTime;
			if (moveTimer <= 0) {
				//GameObject.Find("SimplePeople_StreetMan_Brown").GetComponent<Animator>().Play("Death_01");
				GameObject.Find ("Forklift").GetComponent<Animation> ().Play ("forkliftMove");
				moveTimer2 = Random.value * 4 + 5;
			}		
		} else {
			if (moveTimer2 > 0) {
				moveTimer2 -= Time.deltaTime;
				if (moveTimer2 <= 0) {
					Debug.Log("Arrive");
					//GameObject.Find("SimplePeople_StreetMan_Brown").GetComponent<Animator>().Play("Death_01");
					GameObject.Find ("Forklift").GetComponent<Animation> ().Play ("forkliftArrive");
					moveTimer2 = Random.value * 4 + 5;
				}		
			} else {
				//GameObject.Find("SimplePeople_StreetMan_Brown").GetComponent<Animator>().Play("Death_01");
				//smokeTimer = Random.value * 4 + 1.5;
				
			}
		}

		
		if (moveTimer3 > 0) {
			moveTimer3 -= Time.deltaTime;
			if (moveTimer3 <= 0) {
				//GameObject.Find("SimplePeople_StreetMan_Brown").GetComponent<Animator>().Play("Death_01");
				GameObject.Find ("bm_parent").GetComponent<Animation> ().Play ("bmWalk");
				moveTimer3 = Random.value * 4 + 5;
			}		
		} 
	}
}
}