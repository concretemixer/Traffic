using UnityEngine;
using System.Collections;

public class RandomSoundPlay : MonoBehaviour {

    private float toNext = 0;
    public float delayMin = 10;
    public float delayMax = 15;
	// Use this for initialization
	void Start () {
        toNext = Random.Range(delayMin, delayMax);
	}
	
	// Update is called once per frame
	void Update () {
        toNext -= Time.deltaTime;
        if (toNext < 0)
        {
            toNext = Random.Range(delayMin, delayMax);
            this.GetComponent<AudioSource>().Play();
        }
	}
}
