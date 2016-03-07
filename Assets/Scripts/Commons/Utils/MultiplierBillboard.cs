using UnityEngine;
using System.Collections;

public class MultiplierBillboard : MonoBehaviour {

    public Camera m_Camera;
    GameObject myContainer;

    const float v = 5;
    float lifetime = 0;
    const float life = 3;

    void Start()
    {
        myContainer = new GameObject();
        myContainer.name = "GRP_" + transform.gameObject.name;
        myContainer.transform.position = transform.position;
        transform.parent = myContainer.transform;
    }    

    void Update()
    {
        m_Camera = Camera.main;
        if (m_Camera!=null)
        {
            myContainer.transform.LookAt(myContainer.transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
        }

        lifetime += Time.deltaTime;
        myContainer.transform.position = myContainer.transform.position + new Vector3(0,v,0)*Time.deltaTime;
        Color c = GetComponent<TextMesh>().color;
        GetComponent<TextMesh>().color = new Color(c.r, c.g, c.b, 1.0f - ((lifetime / life) * (lifetime / life)));

    }
}
