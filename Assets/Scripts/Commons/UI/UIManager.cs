using UnityEngine;
using System;
using System.Collections.Generic;
using Commons.Resources;

namespace Commons.UI
{
    public class UIManager : IUIManager
    {
        GameObject container;

        private Dictionary<UIMap.Id, GameObject> uiElements = new Dictionary<UIMap.Id, GameObject>();

        IResourceManager resourceManager;

        public void Init(GameObject _container, IResourceManager rm)
        {
            this.container = _container;
            resourceManager = rm;

            if (resourceManager == null)
                Debug.Log("NULLLLL !!");
            else
                Debug.Log("OK !!");
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