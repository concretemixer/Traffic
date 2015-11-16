using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level : MonoBehaviour {

	GameObject cameraPortrait;
	GameObject cameraLandscape;

	public bool Crash = false;
	public bool Complete = false;
	public bool PreStart = true;

	public string NextLevelName = "";

	public int score = 0;
	public int scoreFast = 0;
	public int targetScore = 50;

	private bool success = false;

	public Transform pitcher;

	public bool Ingame = false;

	// Use this for initialization
	void Start () {

		cameraPortrait = GameObject.Find("Main Camera Portrait");
		cameraLandscape = GameObject.Find("Main Camera Landscape");;

		UpdateCamera ();
		//Time.timeScale = 3;
		Ingame = GameObject.Find ("Game") != null;

		if (Ingame) {
			GameObject.Find ("Back").GetComponent<Animator> ().Play ("fade_back");
			GameObject.Find ("Back 1").GetComponent<Animator> ().Play ("fade_back");
			GameObject.Find ("Back 2").GetComponent<Animator> ().Play ("fade_back");
			if (GameObject.Find ("ProgressSlider") != null) {
				GameObject.Find ("ProgressSlider").GetComponent<Slider> ().maxValue = targetScore;
				UpdateScore ();
			}
			if (GameObject.Find ("MusicSource").GetComponent<AudioSource> ().isPlaying) {
				//GameObject.Find ("AmbSource").GetComponent<AudioSource> ().Pause ();		
			} else {
			//	GameObject.Find ("AmbSource").GetComponent<AudioSource> ().Play ();				
			}
		}
	//	else
	//		GameObject.Find ("AmbSource").GetComponent<AudioSource> ().Play ();				


		PreStart = true;
		Invoke ("StartDelay", 1);
	}

	void StartDelay()
	{


		PreStart = false;
	}

	// Update is called once per frame
	void Update () {

		UpdateCamera ();

		if (Complete && !success) {
			if (GameObject.FindGameObjectsWithTag("Vehicle").Length == 0) {
				GameObject.Find ("Game").GetComponent<Game> ().OnSuccess (scoreFast,score );
				success = true;
			}
		}
	}

	void UpdateCamera ()
	{
		if (cameraPortrait != null && cameraLandscape != null) {
			if ((Screen.orientation == ScreenOrientation.LandscapeRight) ||  (Screen.orientation == ScreenOrientation.LandscapeLeft))
			{
				Debug.Log("landscape");
				cameraPortrait.SetActive(false);
				cameraLandscape.SetActive(true);
			}
			
			if ((Screen.orientation == ScreenOrientation.PortraitUpsideDown ) ||  (Screen.orientation == ScreenOrientation.Portrait))
			{
				Debug.Log("portrait");
				cameraPortrait.SetActive(true);
				cameraLandscape.SetActive(false);
			}		
		}
	}

	public void OnCrash()
	{
		if (!Crash) {
			GameObject o = GameObject.Find ("Game");
			if (o!=null)
				o.GetComponent<Game> ().OnFailed ();
		}
		Crash = true;
	}

	public void OnReach(Vehicle v)
	{
		if (Crash)
			return;

		if (!v.IsBus) {
			score++;
			if (v.gear == 2 && !Complete)
				scoreFast++;
		}
		if (score >= targetScore) {
			score = targetScore;
			Complete = true;
		}

		UpdateScore();
	}
	
	private void UpdateScore()
	{
		if (GameObject.Find ("ProgressSlider") != null) {
			GameObject.Find ("Score").GetComponent<Text> ().text = score.ToString () + "/" + targetScore.ToString ();
			GameObject.Find ("ProgressSlider").GetComponent<Slider> ().value = score;
		}
	}

	public void Restart()
	{
		Crash = false;
		score = 0;
		scoreFast = 0;
		UpdateScore ();

		foreach (var go in GameObject.FindGameObjectsWithTag("Vehicle")) {
			Destroy(go);
		}
	}

	public void OnApplicationQuit () {
		PlayerPrefs.SetInt("Screenmanager Resolution Width", 960);
		PlayerPrefs.SetInt("Screenmanager Resolution Height", 600);
		PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
	}
}
