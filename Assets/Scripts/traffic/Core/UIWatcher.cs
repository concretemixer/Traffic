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

                float ratio = (float)Screen.height / (float)Screen.width;

                if (ratio < 1)
                    uiRoot.GetComponent<CanvasScaler>().referenceResolution = new Vector2(960, 960 * ratio);
                else
                    uiRoot.GetComponent<CanvasScaler>().referenceResolution = new Vector2(960 / ratio, 960);

                orientation = Screen.orientation;
                onOrientationChanged.Dispatch();                
            }
        }
	}
}
