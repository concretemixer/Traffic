using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core {

public class Level : MonoBehaviour  {
 /*
    [Inject]
    public VehicleReachedDestination onVehicleReachedDestination { get; set; }

    [Inject]
    public VehicleCrashed onVehicleCrashed { get; set; }

    [Inject]
    public LevelFailed onLevelFailed { get; set; }

    [Inject]
    public LevelComplete onLevelComplete { get; set; }

    [Inject]
    public LevelPaused onLevelPaused { get; set; }
   */
	GameObject cameraPortrait;
	GameObject cameraLandscape;
	GameObject cameraMain;

	//  bool Crash = false;
	//  bool Complete = false;
	//  bool PreStart = true;	

	// Use this for initialization
	void Start () {
		Debug.Log ("start");
		

	//	cameraMain = GameObject.Find("UI Camera");

        GameObject levelRoot = this.transform.parent.gameObject;



        cameraPortrait = levelRoot.transform.FindChild("Main Camera Portrait").gameObject;
        cameraLandscape = levelRoot.transform.FindChild("Main Camera Landscape").gameObject;

		{
            if (cameraMain != null)
            {
             //   cameraMain.GetComponent<Camera>().enabled = false;
             //   cameraMain.GetComponent<AudioListener>().enabled = false;
            }
			if (cameraPortrait != null) {
				GameObject o = new GameObject ("listener");
				o.AddComponent<AudioListener> ();
				o.transform.parent = cameraPortrait.transform;
				o.transform.position = new Vector3 (0, 20, 0);
				o.transform.rotation = o.transform.parent.rotation;
			}
			if (cameraLandscape != null) {
				GameObject o = new GameObject ("listener");
				o.AddComponent<AudioListener> ();
				o.transform.parent = cameraLandscape.transform;
				o.transform.position = new Vector3 (0, 20, 0);
				o.transform.rotation = o.transform.parent.rotation;
			}
        }



#if UNITY_STANDALONE

        if (Screen.width > Screen.height)
        {
            cameraPortrait.SetActive(false);
            cameraLandscape.SetActive(true);
            cameraPortrait = null;            
        }
        else
        {
            cameraPortrait.SetActive(true);
            cameraLandscape.SetActive(false);            
            cameraLandscape = null;

        }
#endif


        UpdateCamera ();
		//Time.timeScale = 3;
			/*
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

*/

        float musicVolume = PlayerPrefs.GetFloat("volume.music", 1);
        if (musicVolume == 0)
        {
            float soundVolume = PlayerPrefs.GetFloat("volume.sound", 1);
            GameObject snd = GameObject.Find("AmbientSound");
            if (snd != null)
            {
                snd.GetComponent<AudioSource>().volume = soundVolume;
            }
        }

	}


	// Update is called once per frame
	void Update () {

		UpdateCamera ();

        /*
		if (Complete && !success) {
			if (GameObject.FindGameObjectsWithTag("Vehicle").Length == 0) {
				GameObject.Find ("Game").GetComponent<Game> ().OnSuccess (scoreFast,score );
				success = true;
			}
		}
         * */
	}

	void UpdateCamera ()
	{
		if (cameraPortrait != null && cameraLandscape != null) {
			if ((Screen.orientation == ScreenOrientation.LandscapeRight) ||  (Screen.orientation == ScreenOrientation.LandscapeLeft))
			{
			
				cameraPortrait.SetActive(false);
				cameraLandscape.SetActive(true);
			}
			
			if ((Screen.orientation == ScreenOrientation.PortraitUpsideDown ) ||  (Screen.orientation == ScreenOrientation.Portrait))
			{

				cameraPortrait.SetActive(true);
				cameraLandscape.SetActive(false);
			}		
		}


	}
	    /*
	public void Restart()
	{
		Crash = false;
		score = 0;
		scoreFast = 0;

		foreach (var go in GameObject.FindGameObjectsWithTag("Vehicle")) {
			Destroy(go);
		}
	}     */

	public void OnApplicationQuit () {
		PlayerPrefs.SetInt("Screenmanager Resolution Width", 960);
		PlayerPrefs.SetInt("Screenmanager Resolution Height", 600);
		PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
	}
}

}