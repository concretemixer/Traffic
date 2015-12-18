using UnityEngine;
using System.Collections;

public class PoleMovement : MonoBehaviour {

	// Use this for initialization
    float lifetime = 0;
    bool down = false;

	void Start () {
        transform.localRotation = Quaternion.EulerAngles(0, 0, -1);
        transform.localPosition = Vector3.zero;
        lifetime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        lifetime += Time.deltaTime;
        if (down)
        {           
            if (lifetime > 14.5)
            {
                transform.localRotation = Quaternion.EulerAngles(0, 0, -(lifetime-14.5f));
            }

            if (lifetime > 15.5)
            {
                transform.localRotation = Quaternion.EulerAngles(0, 0, -1);                
            }
        }
        else
        {
            
            if (lifetime > 6.5f)
            {
                transform.localRotation = Quaternion.EulerAngles(0, 0, -(7.5f - lifetime));
            }

            if (lifetime > 7.5f)
            {
                transform.localRotation = Quaternion.EulerAngles(0, 0, 0);
                down = true;
            }
        }
	}
}
