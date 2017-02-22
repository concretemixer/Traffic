using UnityEngine;
using System;
using System.Collections.Generic;
using Commons.Resources;
using Traffic.MVCS.Commands.Signals;
using Traffic.MVCS.Models;
using Traffic.MVCS.Views.UI;
using Commons.Utils;

namespace Commons.UI
{
    public class UIManager : IUIManager
    {
        [Inject]
        public OrientationChangedSignal onOrientationChanged { get; set; }

        [Inject]
        public ILocaleService localeService { get; set; }


        GameObject container;

        private Dictionary<UIMap.Id, GameObject> uiElements = new Dictionary<UIMap.Id, GameObject>();

        IResourceManager resourceManager;

        public void Init(GameObject _container, IResourceManager rm)
        {
            this.container = _container;
            resourceManager = rm;

            onOrientationChanged.AddListener(handleOrientationChange);
        }

        void handleOrientationChange()
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
            foreach (var key in uiElements.Keys)
            {
                if (uiElements[key].GetComponent<RotatableView>()!=null)
                {
                    uiElements[key].GetComponent<RotatableView>().Layout(w,h);
                }
            }
        }

        public TViewClass Get<TViewClass>(UIMap.Id _viewId) 
        {
            if (uiElements.ContainsKey(_viewId))
                return uiElements[_viewId].GetComponent<TViewClass>();
            return default(TViewClass);
        }

        public TViewClass Show<TViewClass>(UIMap.Id _viewId)
        {
            var instance = Show(_viewId);
            return instance.GetComponent<TViewClass>();
        }

        public GameObject Show(UIMap.Id _viewId)
        {
            Loggr.Log("attempt to instantiate: " + _viewId.ToString());
            
            if (resourceManager == null)
                Debug.Log("NULLLLL");

            if (uiElements.ContainsKey(_viewId))
            {
                Loggr.Log("already exist: " + _viewId.ToString());
                return uiElements[_viewId];
            }

            var resourcePath = UIMap.GetPath(_viewId);
            var view = resourceManager.GetResource<GameObject>(resourcePath);

            var instance = GameObject.Instantiate(view as GameObject) as GameObject;
            instance.transform.SetParent(container.transform);
    	    instance.transform.localPosition = Vector3.zero;

            localeService.SetAllTexts(instance.gameObject);

            uiElements[_viewId] = instance;

            return instance;
        }

        public void Hide(UIMap.Id _viewId)
        {
            if (uiElements.ContainsKey(_viewId))
            {
                GameObject.Destroy(uiElements[_viewId]);
                uiElements.Remove(_viewId);
            }     
        }

        public void HideAll()
        {
            foreach(var id in uiElements.Keys)
                GameObject.Destroy(uiElements[id]);

            uiElements.Clear();
        }
    }
}