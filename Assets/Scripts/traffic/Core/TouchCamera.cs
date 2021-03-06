﻿using UnityEngine;
using System.Collections;
using System.IO;

namespace Traffic.Core
{

    public class TouchCamera : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
        }


        Vehicle touchedVehicle = null;
        Vector2 touchedVehiclePos = new Vector2(0, 0);

        int shotNum = 0;

        // Update is called once per frame
        void Update()
        {

            Vector2 position = new Vector2(0, 0);


            
            if (Input.GetKeyDown(KeyCode.S))
            {

                string filename = @"d:\--\shot" + shotNum.ToString() + ".png";
                while (File.Exists(filename))
                {
                    shotNum++;
                    filename = @"d:\--\shot" + shotNum.ToString() + ".png";                    
                }

                Application.CaptureScreenshot(filename);
                shotNum++;
            }

            if (Time.timeScale == 0)
                return;

            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    //Vector3 newVehiclePos = Camera.main.WorldToScreenPoint(touchedVehicle.transform.position);

                    //newVehiclePos - touchedVehiclePos

                    if (touchedVehicle != null)
                    {

                        Vector2 l = Input.GetTouch(0).position - touchedVehiclePos;

                        if (l.magnitude < 50)
                            touchedVehicle.SpeedUp();
                        else
                            touchedVehicle.SlowDown();

                        touchedVehicle = null;
                    }
                }
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    position = Input.GetTouch(0).position;
                    Ray ray = Camera.main.ScreenPointToRay(position);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        //Debug.Log(hit.transform.gameObject.name);
                        if (hit.transform.gameObject.tag == "Vehicle")
                        {
                            touchedVehicle = hit.transform.GetComponent<Vehicle>();
                            touchedVehiclePos = position;
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Debug.Log(Camera.main.gameObject.name);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    //Debug.Log(hit.transform.gameObject.name);
                    if (hit.transform.gameObject.tag == "Vehicle")
                    {
                        Vehicle vehicle = hit.transform.GetComponent<Vehicle>();
                        if (Input.GetMouseButtonDown(1))
                            vehicle.SlowDown();
                        if (Input.GetMouseButtonDown(0))
                            vehicle.SpeedUp();
                    }
                }
            }
            else
                return;


        }
    }
}