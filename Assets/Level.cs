using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level : MonoBehaviour {

	public bool Crash = false;
	public bool Complete = false;

	public int score = 0;
	public int targetScore = 50;

	private bool success = false;

	public Transform pitcher;

	GameObject uiSuccess = null;
	GameObject uiRestart = null;
	// Use this for initialization
	void Start () {
		uiSuccess = GameObject.Find ("Success");
		uiSuccess.SetActive (false);
		uiRestart = GameObject.Find ("Restart");
		uiRestart.SetActive (false);

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

		uiSuccess.GetComponent<Text>().text = "FAIL!!";
		uiSuccess.SetActive (true);
		uiRestart.SetActive (true);
	}

	public void OnReach(Vehicle v)
	{
		if (Crash)
			return;
		score++;
		if (score >= targetScore) {
			score = targetScore;
			Complete = true;
		}

		UpdateScore();
	}

	public void OnSuccess()
	{
		uiSuccess.GetComponent<Text>().text = "Success!";
		uiSuccess.SetActive (true);
	}

	private void UpdateScore()
	{
		GameObject.Find("Score").GetComponent<Text>().text = score.ToString()+" / "+targetScore.ToString();
	}

	public void Restart()
	{
		uiSuccess.SetActive (false);
		uiRestart.SetActive (false);
		Crash = false;
		score = 0;
		UpdateScore ();

		foreach (var go in GameObject.FindGameObjectsWithTag("Vehicle")) {

			Destroy(go);

		}
	}
}
