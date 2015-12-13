using UnityEngine;
using UnityEngine.UI;
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

        ScreenOrientation orientation;

        void Start() 
        {
            orientation = Screen.orientation;
        }

        void Update()
        {
            if (orientation != Screen.orientation)
            {

                float ratio = (float)Screen.height / (float)Screen.width;

                uiRoot.GetComponent<CanvasScaler>().referenceResolution = new Vector2(960, 960 * ratio);
                orientation = Screen.orientation;
                onOrientationChanged.Dispatch();

                Debug.Log("onOrientationChanged");
            }
        }

	}
}
