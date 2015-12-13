using UnityEngine;
using System;
using System.Collections.Generic;

namespace Commons.UI
{
    public class UIManager : IUIManager
    {
        GameObject container;

		[Inject]
		public UIMap uiMap { get; set;}

        private Dictionary<UIMap.Id, GameObject> uiElements = new Dictionary<UIMap.Id, GameObject>();

        public void Init(GameObject _container)
        {
            this.container = _container;
        }

        public TViewClass Show<TViewClass>(UIMap.Id _viewId)
        {
            var instance = Show(_viewId);
            return instance.GetComponent<TViewClass>();
        }

        public GameObject Show(UIMap.Id _viewId)
        {
            var resourcePath = uiMap.GetPath(_viewId);
			var prefab = (GameObject)UnityEngine.Resources.Load(resourcePath);

            var instance = GameObject.Instantiate(prefab as GameObject) as GameObject;
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