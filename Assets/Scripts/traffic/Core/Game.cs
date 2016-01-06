using UnityEngine;
//using UnityEditor;
using UnityEngine.UI;
using System.Collections;

namespace Traffic.Core {

public class Game : MonoBehaviour {

	GameObject endLevelPanel = null;
	GameObject optionsUiRoot = null;
	GameObject pauseButton = null;
	GameObject goButton = null;
	GameObject nextButton = null;
	GameObject menuButton = null;
	GameObject preUiRoot = null;
	GameObject uiRoot = null;
	GameObject startUiRoot = null;

	GameObject page1 = null;
	GameObject page2 = null;

	GameObject cameraMenu = null;

	int currentLevelIndex = 0;
	string[] levels = new string[] 
	{
		"Level1",
		"Level2",
		"Level4_1",

		"Level3_1",
		"Level4",
		"Level10",

		"Level6",
		"Level5",
		"Level3",

		"Level18_1",
		"Level8",
		"Level7",


		"Level21",
		"Level13_1",
		"Level9",

		"Level11",
		"Level12",
		"Level14",

		"Level15",
		"Level13",	
		"Level16",

		"Level17",
		"Level18",
		"Level19",
	};

	int[] levelState = new int[] {
/*		1,1,1,1,1,
		1,1,1,1,1,
		1,1,1,1,1,
		1,1,1,1,0};
*/
		0,-1,-1,-1,-1,
		-1,-1,-1,-1,-1,
		-1,-1,-1,-1,-1,
		-1,-1,-1,-1,-1,
		-1,-1,-1,-1};

	void UpdateLevelProgress()
	{
		//UnityEngine.Object[] allSprites = AssetDatabase.LoadAllAssetRepresentationsAtPath("Assets/ui/stars.png");

		//levelState = new int[20];



		for (int a=0; a<24; a++) {
			levelState[a] = PlayerPrefs.GetInt("progress.1."+(a+1).ToString(),0);
		}
		if (levelState[0]==-1)
			levelState[0]=0;

		for (int a=0; a<24; a++) {
			GameObject b = GameObject.Find ("ButtonLevel"+(a+1).ToString()).gameObject;


			if (b.transform.FindChild("ImageProgress")==null)
				continue;

			if (b!=null) {
				GameObject img = b.transform.FindChild("ImageProgress").gameObject;
				GameObject img2 = b.transform.FindChild("ImageLock").gameObject;

				if (levelState[a]>0) {
					if (levelState[a]==1) 										
						img.GetComponent<RawImage>().uvRect = new Rect(0.5f,0.5f,0.5f,0.5f);
					if (levelState[a]==2) 										
						img.GetComponent<RawImage>().uvRect = new Rect(0.0f,0.0f,0.5f,0.5f);
					if (levelState[a]==3) 										
						img.GetComponent<RawImage>().uvRect = new Rect(0.5f,0.0f,0.5f,0.5f);
					img2.GetComponent<Image>().color = new Color32(255,255,255,0);
					img.GetComponent<RawImage>().color = new Color32(255,255,255,255);
					//img.GetComponent<Image>().sprite = allSprites[levelState[a]]  as Sprite;
					b.GetComponent<Button>().interactable = true;
				}
				else if (levelState[a]==0) {
					img.GetComponent<RawImage>().uvRect = new Rect(0.0f,0.5f,0.5f,0.5f);
					img2.GetComponent<Image>().color = new Color32(255,255,255,0);
					img.GetComponent<RawImage>().color = new Color32(255,255,255,255);
					b.GetComponent<Button>().interactable = true;

				}
				else {
					img2.GetComponent<Image>().color = new Color32(255,255,255,200);
					img.GetComponent<RawImage>().color = new Color32(255,255,255,0);
					b.GetComponent<Button>().interactable = false;
				}
				
			}
		}

		
	}
	// Use this for initialization
	void Start () {
			

		Object.DontDestroyOnLoad(this);
		Object.DontDestroyOnLoad(GameObject.Find("MusicSource"));
		Object.DontDestroyOnLoad(GameObject.Find("Canvas"));
		Object.DontDestroyOnLoad(GameObject.Find("Canvas 1"));
		//Object.DontDestroyOnLoad(GameObject.Find("Helpers"));
		Object.DontDestroyOnLoad(GameObject.Find("Camera2"));

		cameraMenu = GameObject.Find ("Camera2");

		//Application.LoadLevel ("Level0");


	//	GameObject.Find ("Canvas 1").transform.SetAsFirstSibling ();

		UpdateLevelProgress();


		page1 = GameObject.Find ("Page1");
		page2 = GameObject.Find ("Page2");
		
		endLevelPanel = GameObject.Find ("EndLevelPanel");
		pauseButton = GameObject.Find ("PauseButton");
		goButton = GameObject.Find ("GoButton");
		nextButton = GameObject.Find ("NextButton");
		menuButton = GameObject.Find ("MenuButton");

		endLevelPanel.SetActive(false);			
		goButton.SetActive (false);
		page2.SetActive (false);

		nextButton.GetComponent<Button> ().interactable = false;

		//Application.LoadLevel(levels[currentLevelIndex]);
		//GameObject.Find ("LevelNum").GetComponent<Text> ().text = (currentLevelIndex + 1).ToString ();

		float ratio = (float)Screen.width / (float)Screen.height;

		uiRoot = GameObject.Find ("IngameUIRoot");
		uiRoot.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 600.0f * ratio);
		uiRoot.SetActive (false);

		preUiRoot = GameObject.Find ("PreUIRoot");
		preUiRoot.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 600.0f * ratio);
		preUiRoot.SetActive (false);

		startUiRoot = GameObject.Find ("StartUIRoot");
		startUiRoot.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 600.0f * ratio);
		startUiRoot.SetActive (false);

		optionsUiRoot = GameObject.Find ("OptionsUIRoot");
		optionsUiRoot.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 600.0f * ratio);
		optionsUiRoot.SetActive (false);

//		GameObject.Find ("Back").GetComponent<Animator> ().Play ("show_back");
		GameObject.Find ("Back 1").GetComponent<Animator> ().Play ("show_back");
		GameObject.Find ("Back 2").GetComponent<Animator> ().Play ("show_back");




		Invoke ("ShowStartUI", 1);
	}

	void ShowStartUI()
	{
		optionsUiRoot.SetActive (true);
		startUiRoot.SetActive (true);
		GameObject.Find ("MusicSource").GetComponent<AudioSource> ().Play ();
	}


	// Update is called once per frame
	void Update () {

	}

	private void OnMenuDelay()
	{
		//uiRoot.SetActive (false);
		preUiRoot.SetActive (true);
		startUiRoot.SetActive (false);

		bool page = page1.activeSelf;

		page1.SetActive (true);
		page2.SetActive (true);

		UpdateLevelProgress();

		page1.SetActive (page);
		page2.SetActive (!page);

	}

	public void OnMenuPressed()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Level0");
		cameraMenu.SetActive (true);
		GameObject.Find ("Canvas 1").GetComponent<Canvas> ().renderMode = RenderMode.ScreenSpaceCamera;
		Time.timeScale = 1;
		//endLevelPanel.GetComponent<Animation> ().Play ("hide_panel");
		endLevelPanel.SetActive(false);
		uiRoot.SetActive (false);
		optionsUiRoot.SetActive (true);
		//preUiRoot.SetActive (true);
		//startUiRoot.SetActive (false);
		GameObject.Find ("Back").GetComponent<Animator> ().Play ("show_back");
		GameObject.Find ("Back 1").GetComponent<Animator> ().Play ("show_back");
		GameObject.Find ("Back 2").GetComponent<Animator> ().Play ("show_back");
		Invoke("OnMenuDelay", 1);
	}

	public void OnBackPressed()
	{
		uiRoot.SetActive (false);
		preUiRoot.SetActive (false);
		startUiRoot.SetActive (true);
	}

	public void OnStartPressed()
	{
		uiRoot.SetActive (false);
		preUiRoot.SetActive (true);
		startUiRoot.SetActive (false);
	}

	public void OnPausePressed()
	{
		Time.timeScale = 0;
		endLevelPanel.SetActive(true);
		optionsUiRoot.SetActive (true);
		//endLevelPanel.GetComponent<Animation> ().Play ("pop_panel");
		goButton.SetActive (true);
		pauseButton.SetActive (false);
	}

	public void OnGoPressed()
	{
		Time.timeScale = 1;
		endLevelPanel.SetActive(false);
		optionsUiRoot.SetActive (false);
		//endLevelPanel.GetComponent<Animation> ().Play ("hide_panel");
		goButton.SetActive (false);
		pauseButton.SetActive (true);
	}

	public void OnRestartPressed()
	{
		Time.timeScale = 1;
		//endLevelPanel.GetComponent<Animation> ().Play ("hide_panel");
		endLevelPanel.SetActive(false);		
		goButton.SetActive (false);
		pauseButton.SetActive (true);

		//GameObject.Find ("Level").GetComponent<Level> ().Restart ();
	}

	public void OnNextPressed()
	{
		Time.timeScale = 1;
		//endLevelPanel.GetComponent<Animation> ().Play ("hide_panel");
		endLevelPanel.SetActive(false);		
		goButton.SetActive (false);
		pauseButton.SetActive (true);

		if (currentLevelIndex == levelState.Length - 1) {
			OnMenuPressed();
		} else {
			currentLevelIndex++;
			UnityEngine.SceneManagement.SceneManager.LoadScene(levels [currentLevelIndex]);
			nextButton.GetComponent<Button> ().interactable = false;

			GameObject.Find ("LevelNum").GetComponent<Text> ().text = (currentLevelIndex + 1).ToString ();
		}
	}

	public void OnPrevPressed()
	{
		Time.timeScale = 1;
		endLevelPanel.SetActive(false);		
		goButton.SetActive (false);
		pauseButton.SetActive (true);
		
		currentLevelIndex--;
		UnityEngine.SceneManagement.SceneManager.LoadScene(levels[currentLevelIndex]);
		nextButton.GetComponent<Button> ().interactable = false;

		GameObject.Find ("LevelNum").GetComponent<Text> ().text = (currentLevelIndex + 1).ToString ();
	}

	public void OnSuccessDelay()
	{
		endLevelPanel.SetActive (true);		
		optionsUiRoot.SetActive (true);	
		goButton.SetActive (false);
		pauseButton.SetActive (false);
	}

	public void OnSuccess(int fast, int score)
	{
		//endLevelPanel.GetComponent<Animation> ().Play ("pop_panel");
		Debug.LogFormat ("{0} / {1}", fast, score);
		float k = (float)fast / (float)score;
		int stars = 1;
		if (k >= 0.8)
			stars = 3;
		else if (k>=0.5)
			stars = 2;


		int state = PlayerPrefs.GetInt("progress.1."+(currentLevelIndex+1).ToString(),-1);
		if (state<stars)
			PlayerPrefs.SetInt("progress.1."+(currentLevelIndex+1).ToString(),stars);
		
		state = PlayerPrefs.GetInt("progress.1."+(currentLevelIndex+2).ToString(),-1);
		if (state==-1)
			PlayerPrefs.SetInt("progress.1."+(currentLevelIndex+2).ToString(),0);

		nextButton.GetComponent<Button> ().interactable = true;
		nextButton.transform.SetAsLastSibling ();

		if (currentLevelIndex == levelState.Length - 1) {
			menuButton.GetComponent<Button> ().interactable = false;
			menuButton.transform.SetAsFirstSibling ();	
		}

		GameObject img = endLevelPanel.transform.FindChild("ImageProgressCur").gameObject;
		img.GetComponent<RawImage> ().color = new Color (1, 1, 1, 1);
		if (stars>0) {
			if (stars==1) 										
				img.GetComponent<RawImage>().uvRect = new Rect(0.5f,0.5f,0.5f,0.5f);
			if (stars==2)
				img.GetComponent<RawImage>().uvRect = new Rect(0.0f,0.0f,0.5f,0.5f);
			if (stars==3)
				img.GetComponent<RawImage>().uvRect = new Rect(0.5f,0.0f,0.5f,0.5f);
		}

		Invoke("OnSuccessDelay", 1);	


	}

	private void OnFailedDelay() 
	{
		endLevelPanel.SetActive (true);		
		optionsUiRoot.SetActive (true);
		//endLevelPanel.GetComponent<Animation> ().Play ("pop_panel");
	}


	public void OnFailed()
	{
		nextButton.GetComponent<Button> ().interactable = false;
		nextButton.transform.SetAsFirstSibling ();
		goButton.SetActive (false);
		pauseButton.SetActive (false);	
		GameObject img = endLevelPanel.transform.FindChild("ImageProgressCur").gameObject;		
		img.GetComponent<RawImage> ().color = new Color (1, 1, 1, 0);
		Invoke("OnFailedDelay", 1);
	}

	public void OnLevelSelect(int level)
	{
		optionsUiRoot.SetActive (false);
		uiRoot.SetActive (true);
		GameObject.Find ("PreUIRoot").SetActive (false);

		Time.timeScale = 1;
		endLevelPanel.SetActive(false);		
		goButton.SetActive (false);
		pauseButton.SetActive (true);
		
		currentLevelIndex = level;
		GameObject.Find ("Canvas 1").GetComponent<Canvas> ().renderMode = RenderMode.ScreenSpaceOverlay;
		cameraMenu.SetActive (false);
		UnityEngine.SceneManagement.SceneManager.LoadScene(levels[currentLevelIndex]);
		nextButton.GetComponent<Button> ().interactable = false;

		menuButton.GetComponent<Button> ().interactable = true;
		menuButton.transform.SetAsLastSibling ();


		GameObject.Find ("LevelNum").GetComponent<Text> ().text = (currentLevelIndex + 1).ToString ();

	}

	public void OnQuitPressed()
	{
		Application.Quit ();
	}

	public void OnSoundToggle(bool value)
	{
		if (value) {
			// off
		} else {
			//on
		}

	}

	public void OnMisicToggle(bool value)
	{
		if (value) {
			if (GameObject.Find ("AmbSource")!=null)
				GameObject.Find ("AmbSource").GetComponent<AudioSource> ().Play ();
			GameObject.Find ("MusicSource").GetComponent<AudioSource> ().Pause ();
		} else {
			if (GameObject.Find ("AmbSource")!=null)
				GameObject.Find ("AmbSource").GetComponent<AudioSource> ().Pause ();
			GameObject.Find ("MusicSource").GetComponent<AudioSource> ().Play ();
		}
	}

	public void OnPrevPage()
	{
		page2.SetActive (false);
		page1.SetActive (true);
	}

	public void OnNextPage()
	{
		page1.SetActive (false);
		page2.SetActive (true);
	}

}
}