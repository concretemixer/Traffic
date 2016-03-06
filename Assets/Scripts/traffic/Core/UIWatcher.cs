using UnityEngine;
using UnityEngine.UI;
using Commons.UI;
using Traffic.Components;
using System;
using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core {

	class UIWatcher : MonoBehaviour
	{
        [Inject]
        public OrientationChangedSignal onOrientationChanged { get; set; }

        [Inject(EntryPoint.Container.UI)]
        public GameObject uiRoot { get; set; }




        ScreenOrientation orientation = ScreenOrientation.Unknown;

        void Start() 
        {
            //orientation = Screen.orientation;
           // SetCanvasRatio();
        }

        void Update()
        {
            if (orientation != Screen.orientation)
            {
                int w = Screen.width;
                int h = Screen.height;

                if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
                {
                    w = Math.Min(Screen.width, Screen.height);
                    h = Math.Max(Screen.width, Screen.height);
                }
                if (Screen.orientation == ScreenOrientation.Landscape || Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
                {
                    w = Math.Max(Screen.width, Screen.height);
                    h = Math.Min(Screen.width, Screen.height);
                }
#if UNITY_STANDALONE
                w = Screen.width;
                h = Screen.height;
#endif

                   
                float ratio = (float)h / (float)w;

                var scaler = uiRoot.GetComponent<CanvasScaler>();
                if(scaler == null) return; 
                
                if (ratio < 1)
                    scaler.referenceResolution = new Vector2(960, 960 * ratio);
                else
                    scaler.referenceResolution = new Vector2(960 / ratio, 960);

                orientation = Screen.orientation;
                onOrientationChanged.Dispatch();                
            }
        }
	}
}
