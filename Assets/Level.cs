using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level : MonoBehaviour {

	public bool Crash = false;
	public bool Complete = false;

	public int score = 0;
	public int targetScore = 100;

	private bool success = false;

	public Transform pitcher;

	GameObject uiSuccess = null;
	// Use this for initialization
	void Start () {
		uiSuccess = GameObject.Find ("Success");
		uiSuccess.SetActive (false);
		UpdateScore();
	}
	
	// Update is called once per frame
	void Update () {
		if (Complete && !success) {
			if (GameObject.FindGameObjectsWithTag("Vehicle").Length == 0) {
				OnSuccess();
				success = true;
			}
		}
	}

	public void OnCrash()
	{
		Crash = true;
	}

	public void OnReach(Vehicle v)
	{
		score++;
		if (score >= targetScore) {
			score = targetScore;
			Complete = true;
		}

		UpdateScore();
	}

	public void OnSuccess()
	{
		uiSuccess.SetActive (true);
	}

	private void UpdateScore()
	{
		GameObject.Find("Score").GetComponent<Text>().text = score.ToString()+" / "+targetScore.ToString();
	}
}
