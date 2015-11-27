using Commons.Resources;
using UnityEngine;

namespace Commons.UI
{
    public class UIManager
    {
        GameObject container;

        UIMap uiMap;

        IResourceManager resourceManager;

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
            var view = resourceManager.GetResource<GameObject>(resourcePath);

            GameObject instance = Object.Instantiate(view);
            instance.transform.SetParent(container.transform);

            return instance;
        }
    }
}