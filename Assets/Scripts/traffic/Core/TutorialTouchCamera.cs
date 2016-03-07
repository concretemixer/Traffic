using UnityEngine;
using System.Collections;
using System.IO;
using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core
{

    public class TutorialTouchCamera : MonoBehaviour
    {

        [Inject]
        public ResumeTutorial onResumeTutorial { get; set; }

        Vehicle target = null;
        bool accelerate = false;

        public void SetTarget(Vehicle t, bool acc = false)
        {
            target = t;
            accelerate = acc;
        }


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


            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    //Vector3 newVehiclePos = Camera.main.WorldToScreenPoint(touchedVehicle.transform.position);

                    //newVehiclePos - touchedVehiclePos

                    if (touchedVehicle != null)
                    {
                        bool blockAccel = false;
                        bool blockDecel = false;

                        if (Time.timeScale == 0)
                        {
                            blockAccel = !accelerate;
                            blockDecel = accelerate;
                        }

                        Vector2 l = Input.GetTouch(0).position - touchedVehiclePos;

                        if (l.magnitude < 50)
                        {
                            if (!blockAccel)
                            {
                                touchedVehicle.SpeedUp();
                                onResumeTutorial.Dispatch();
                            }
                        }
                        else
                        {
                            if (!blockDecel)
                            {
                                touchedVehicle.SlowDown();
                                onResumeTutorial.Dispatch();
                            }
                        }

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

                            if (Time.timeScale == 0)
                            {
                                if (target == null)
                                    touchedVehicle = null;
                                else if (touchedVehicle.Number != target.Number)
                                    touchedVehicle = null;
                            }    
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

                        bool blockAccel = false;
                        bool blockDecel = false;

                        if (Time.timeScale == 0)
                        {
                            if (target == null)
                                vehicle = null;
                            else if (vehicle.Number != target.Number)
                                vehicle = null;

                            blockAccel = !accelerate;
                            blockDecel = accelerate;
                        }

                        if (vehicle != null)
                        {
                            if (Input.GetMouseButtonDown(1) && !blockDecel)
                            {
                                vehicle.SlowDown();
                                onResumeTutorial.Dispatch();
                            }
                            if (Input.GetMouseButtonDown(0) && !blockAccel)
                            {
                                vehicle.SpeedUp();
                                onResumeTutorial.Dispatch();
                            }

                            
                        }
                    }
                }
            }
            else
                return;


        }
    }
}