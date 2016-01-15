using UnityEngine;
using System;
using System.Collections.Generic;
using Commons.Resources;
using Traffic.MVCS.Commands.Signals;
using Traffic.MVCS.Views.UI;

namespace Commons.UI
{
    public class UIManager : IUIManager
    {
        [Inject]
        public OrientationChangedSignal onOrientationChanged { get; set; }

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
            foreach (var key in uiElements.Keys)
            {
                if (uiElements[key].GetComponent<RotatableView>()!=null)
                {
                    uiElements[key].GetComponent<RotatableView>().Layout();
                }
            }
        }

        public TViewClass Show<TViewClass>(UIMap.Id _viewId)
        {
            var instance = Show(_viewId);
            return instance.GetComponent<TViewClass>();
        }

        public GameObject Show(UIMap.Id _viewId)
        {
            if (resourceManager == null)
                Debug.Log("NULLLLL");

            var resourcePath = UIMap.GetPath(_viewId);
            var view = resourceManager.GetResource<GameObject>(resourcePath);

            var instance = GameObject.Instantiate(view as GameObject) as GameObject;
            instance.transform.SetParent(container.transform);
    	    instance.transform.localPosition = Vector3.zero;

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
    }
}