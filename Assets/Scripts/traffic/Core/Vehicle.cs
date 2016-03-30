using UnityEngine;
using System.Collections;
using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core {

public class Vehicle : MonoBehaviour {

    public static int NextNumber = 1;

    public int Number;

    public VehicleReachedDestination onVehicleReachedDestination { get; set; }

    public VehicleCrashed onVehicleCrashed { get; set; }
  
    public LevelComplete onLevelComplete { get; set; }

    public LevelFailed onLevelFailed { get; set; }

    public ScoreGrow onScoreGrow { get; set; }

	public AudioClip[] crashSound;
	public AudioClip moveSound;
	public AudioClip[] accelSound;
	public AudioClip startSound;
	public AudioClip stopSound;

	public float NormalSpeed = 30;
	public float FastSpeed = 60;
	public bool  CanAccelerate = true;
	public bool IsBus = false;

//	private Level level;

	public int gear = 1;
	private float stopTimer = 0;
	private bool stopped = false;

	private bool deceleration = false;
	private float targetSpeed = 0; 
	private float decelRate = 100;

	private bool acceleration = false;
	private float accelRate = 50;

	private float aiStopCooldown = 0;

    private float lifetime = 0;

    private float scoreGrowK = 1;
    private bool finished = false;
    private bool interactable = true;
    private bool crashed = false;

	// Use this for initialization
	void Start () {
		if (tag=="VehicleAI")
			NormalSpeed*=0.75f;

		NormalSpeed += (NormalSpeed * 0.2f) * (Random.value-0.5f);
		FastSpeed += (FastSpeed * 0.2f) * (Random.value-0.5f);

		//Debug.Log ("S: " + NormalSpeed.ToString ());
		if (gear==1)
			GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (NormalSpeed, 0, 0);
		if (gear==2)
			GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (FastSpeed, 0, 0);

		//level = GameObject.Find ("Level").GetComponent<Level>();

		GetComponent<Renderer> ().material.color = new Color (Random.value, (float)(Random.value*0.2f), Random.value);

        ShowEffects();
		if (gameObject.tag == "Vehicle") {
			StopEffects();
		}

        float soundVolume = PlayerPrefs.GetFloat("volume.sound", 1);

		if (gameObject.tag == "Vehicle") {
			if (moveSound != null) {
				Transform t = transform.FindChild ("MoveSource");
				if (t != null && moveSound!=null) {
                    t.gameObject.GetComponent<AudioSource>().PlayOneShot(moveSound, soundVolume);
				}
			}
            onLevelComplete.AddListener(StopOnLevelComplete);
            onLevelFailed.AddListener(StopOnLevelFailed);
        }
	}


    void OnDestroy()
    {
        if (gameObject.tag == "Vehicle")
        {
            onLevelComplete.RemoveListener(StopOnLevelComplete);
            onLevelFailed.RemoveListener(StopOnLevelFailed);
        }
    }

    void StopOnLevelFailed()
    {
        interactable = false;        
    }

    void StopOnLevelComplete() 
    {
        gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        deceleration = true;
        targetSpeed = 0;
    }
	// Update is called once per frame
	void Update () {

        lifetime += Time.deltaTime;

        if (gameObject.tag != "VehicleAI") 
            onScoreGrow.Dispatch(9.0f * Time.deltaTime * scoreGrowK);

		if (stopTimer > 0) {
			stopTimer -= Time.deltaTime;
			if (stopTimer<=0)
				gear = 0;
				SpeedUp();
		}

		if (deceleration) {
			float speed = GetComponent<Rigidbody> ().velocity.magnitude;
			if (speed - decelRate*Time.deltaTime > targetSpeed)
				GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (speed - decelRate*Time.deltaTime, 0, 0);
			else {
				GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (targetSpeed, 0, 0);
				deceleration = false;
			}
		}
		if (acceleration) {
			float speed = GetComponent<Rigidbody> ().velocity.magnitude;
			if (speed + accelRate*Time.deltaTime > targetSpeed)
				GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (speed + accelRate*Time.deltaTime, 0, 0);
			else {
				GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (targetSpeed, 0, 0);
				acceleration = false;
			}
		}

		if (gameObject.tag == "VehicleAI") {
			RaycastHit hit;
			if (gear>0) {
				Ray ray = new Ray(transform.position, transform.rotation * new Vector3 (1, 0, 0));
				if ( Physics.Raycast(ray, out hit)) {
					//Debug.Log(hit.transform.gameObject.name);
					if (hit.transform.gameObject.tag == "VehicleAI" && hit.distance<10)
					{
						SlowDown();
						aiStopCooldown=0.5f;
					}
				}
				ray = new Ray(transform.position, transform.rotation * new Vector3 (1, 0, -0.8f));
				if ( Physics.Raycast(ray, out hit)) {
					//Debug.Log(hit.transform.gameObject.name);
					if (hit.transform.gameObject.tag == "VehicleAI" && hit.distance<15)
					{
						Vector3 dir = hit.transform.rotation * new Vector3 (1, 0, 0);
						if (Vector3.Dot(dir,ray.direction)<0) {
							SlowDown();
							aiStopCooldown=1;
						}
					}
				}/*
				ray = new Ray(transform.position, transform.rotation * new Vector3 (1, 0, -2));
				if ( Physics.Raycast(ray, out hit)) {
					//Debug.Log(hit.transform.gameObject.name);
					if (hit.transform.gameObject.tag == "VehicleAI" && hit.distance<20)
					{
						Vector3 dir = hit.transform.rotation * new Vector3 (1, 0, 0);
						if (Vector3.Dot(dir,ray.direction)<0) {
							SlowDown();
							aiStopCooldown=1;
						}
					}
				}
*/
			}
			else if (gear==0) {
				aiStopCooldown-=Time.deltaTime;
				if (aiStopCooldown<-3.0f)
					SelfDestroy();
				else if (aiStopCooldown<0) {				
					bool go = true;

					Ray ray = new Ray(transform.position, transform.rotation * new Vector3 (1, 0, 0));
					if ( Physics.Raycast(ray, out hit)) {
						//Debug.Log(hit.transform.gameObject.name);
						if (hit.transform.gameObject.tag == "VehicleAI" && hit.distance<10)
						{
							go = false;
						}
					}
					ray = new Ray(transform.position, transform.rotation * new Vector3 (1, 0, -0.8f));
					if ( Physics.Raycast(ray, out hit)) {
						//Debug.Log(hit.transform.gameObject.name);
						if (hit.transform.gameObject.tag == "VehicleAI" && hit.distance<15)
						{
							Vector3 dir = hit.transform.rotation * new Vector3 (1, 0, 0);
							if (Vector3.Dot(dir,ray.direction)<0) {
								go = false;
							}
						}
					}
					ray = new Ray(transform.position, transform.rotation * new Vector3 (1, 0, -2));
					if ( Physics.Raycast(ray, out hit)) {
						//Debug.Log(hit.transform.gameObject.name);
						if (hit.transform.gameObject.tag == "VehicleAI" && hit.distance<20)
						{
							Vector3 dir = hit.transform.rotation * new Vector3 (1, 0, 0);
							if (Vector3.Dot(dir,ray.direction)<0) {
								go = false;
							}
						}
					}


					if (go) {
						SpeedUp();
					}
				}

			}
		}
	}


	void OnTriggerEnter(Collider col)
	{ 
		if(col.gameObject.tag == "Stop")
		{
			if (IsBus && !stopped) {
				int pause = col.GetComponent<BusStop>().GetStopTime();
				stopTimer =  (float)(pause / 1000.0);
				SlowDown();
				gear = -1;
				stopped = true;
			}
		}
		if(col.gameObject.tag == "Yield")
		{
			if (!stopped) {
				//GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
				SlowDown();
				//gear=0;
				stopped = true;
			}
		}
		if(col.gameObject.tag == "Finish" && !finished)
		{
            finished = true;
            //Debug.Log("lifetime = " + lifetime);
            //Debug.Log("F: " + gameObject.name + ":" + Time.time);

            if (gear==2)
                onScoreGrow.Dispatch(100);
            scoreGrowK = 0;

			gameObject.GetComponent<Rigidbody>().detectCollisions = false;
			Invoke("SelfDestroy",3);
			//Destroy(gameObject);
            if (gameObject.tag == "Vehicle") 
                onVehicleReachedDestination.Dispatch();

			Transform t = transform.FindChild("AccelSource");
			if (t!=null) {
				//t.gameObject.GetComponent<AudioSource>().
			}
		}

	}



	void SelfDestroy()
	{
		Destroy(gameObject);
	}

	void OnCollisionEnter (Collision col)
	{
        if (col.gameObject.tag == "Vehicle" || col.gameObject.tag == "Obstacle") 
		{
			GetComponent<Rigidbody> ().drag =10;
			GetComponent<Rigidbody> ().angularDrag = 10;


            if (crashSound != null && !crashed)
            {
                Transform t = transform.FindChild("AccelSource");
                if (t != null && crashSound!=null  && crashSound.Length>0)
                {
                    float soundVolume = PlayerPrefs.GetFloat("volume.sound", 1);
                    t.gameObject.GetComponent<AudioSource>().PlayOneShot(crashSound[Random.Range(0, crashSound.Length)], soundVolume);
                }
                   
                crashed = true;
                if (col.gameObject.GetComponent<Vehicle>()!=null)
                    col.gameObject.GetComponent<Vehicle>().crashed = true;
            }

            onVehicleCrashed.Dispatch();

            Debug.Log(gameObject.name +":"+ Time.time);
		}

		if (col.gameObject.tag == "VehicleAI") 
		{
			GetComponent<Rigidbody> ().drag = 10;
			GetComponent<Rigidbody> ().angularDrag = 10;
			Invoke("SelfDestroy",1);
		}
	}

	public void SlowDown()
	{
        if (!interactable)
            return;

		if (gear == 1) {
			//GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
			deceleration = true;
			targetSpeed = 0;

			if (IsBus) {
				ShowEffects();
			}

		} else if (gear == 2) {
			//GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (NormalSpeed, 0, 0);
			deceleration = true;
			targetSpeed = NormalSpeed;
		}
		else
			return;

		gear--;
		acceleration = false;

        if (gear == 0)
        {
            if (tag != "VehicleAI")                
                onScoreGrow.Dispatch(50);
            scoreGrowK = 0.1f;
        }
        if (gear == 1)
            scoreGrowK = 1;

	}

	void ShowEffects()
	{
        foreach (var p in transform.GetComponentsInChildren<ParticleSystem>())
        {
            p.Play();
        }
                   
		if (transform.Find ("Door") != null)
			transform.Find ("Door").GetComponent<MeshRenderer> ().enabled = true;

	}

	void StopEffects()
	{
        foreach (var p in transform.GetComponentsInChildren<ParticleSystem>())
        {
            p.Stop();
        }

		if (transform.Find ("Door") != null)
			transform.Find ("Door").GetComponent<MeshRenderer> ().enabled = false;

	}

	public void SpeedUp()
	{
        if (!interactable)
            return;

        float soundVolume = PlayerPrefs.GetFloat("volume.sound", 1);

		if (gear == 0) {
			acceleration = true;
			targetSpeed = NormalSpeed;
			//GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (NormalSpeed, 0, 0);
			if (startSound!=null) {
				Transform t = transform.FindChild("AccelSource");
				if (t!=null) {
                    t.gameObject.GetComponent<AudioSource>().PlayOneShot(startSound, soundVolume);
				}
			}

			if (IsBus) {
				StopEffects();
			}
		}
		else if (gear == 1) {
			if (CanAccelerate) {
//				GetComponent<Rigidbody> ().velocity = transform.rotation * new Vector3 (FastSpeed, 0, 0);
				acceleration = true;
				targetSpeed = FastSpeed;
				ShowEffects();

				if (accelSound!=null) {
					Transform t = transform.FindChild("AccelSource");
					if (t!=null) {
                        t.gameObject.GetComponent<AudioSource>().PlayOneShot(accelSound[Random.Range(0, accelSound.Length)], soundVolume);
					}
				}


			}
			else
				return;
		}
		else
			return;
		
		gear++;
		deceleration = false;

        if (gear == 1)
        {
            scoreGrowK = 1;
        }
        else if (gear == 2)
        {
            if (lifetime<1.5)
                scoreGrowK = 20;
            else if (lifetime < 3)
                scoreGrowK = 8;
            else
                scoreGrowK = 4;

          //  Debug.Log(scoreGrowK);
        }

        if (scoreGrowK > 1)
        {
            var m = Resources.Load("tmp/Multiplier");
            GameObject mesh = (GameObject)GameObject.Instantiate(m, transform.position + new Vector3(0, 5, 0), Quaternion.EulerRotation(0, 0, 0));
            mesh.transform.parent = transform.parent;
            if (scoreGrowK>19)
                mesh.GetComponent<TextMesh>().color = Color.yellow;
            else if (scoreGrowK > 7)
                mesh.GetComponent<TextMesh>().color = Color.cyan;
            else 
                mesh.GetComponent<TextMesh>().color = Color.green;

            mesh.GetComponent<TextMesh>().text = "x" + scoreGrowK.ToString("F0");
        }


	}

}

}