using UnityEngine;
using System.Collections;

public class TrainMovement : MonoBehaviour {

    float lifetime = 0;
    private float v = 0;
    private float vmax = 30;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        lifetime += Time.deltaTime;

        if (lifetime < 7.5)
            return;

        v += Time.deltaTime*11;
        if (v > vmax)
            v = vmax;

        Vector3 shift = new Vector3(0,0,-v*Time.deltaTime);
        Vector3 cycle = new Vector3(0, 0, 27.8f);
        for (int a = 0; a < transform.childCount; a++)
        {
            Transform t = transform.GetChild(a);
            t.localPosition += shift;

            if (t.localPosition.z < -27.8f*6)
            {
                if (t.name == "Diesel")
                    Destroy(t.gameObject);
                else
                {
                 //   if (lifetime>12)
                      //  Destroy(t.gameObject);
                   // else
                     //   t.localPosition = cycle;
                }
            }
        }
	}
}
