using Commons.Resources;
using UnityEngine;

namespace Commons.UI
{
    public class UIManager : IUIManager
    {
        GameObject container;

        IResourceManager resourceManager;

        public void Init(GameObject _container, IResourceManager rm)
        {
            this.container = _container;
            resourceManager = rm;
        }

        public TViewClass Show<TViewClass>(UIMap.Id _viewId)
        {
            var instance = Show(_viewId);
            return instance.GetComponent<TViewClass>();
        }

        public GameObject Show(UIMap.Id _viewId)
        {
            var resourcePath = UIMap.GetPath(_viewId);
            var view = resourceManager.GetResource<GameObject>(resourcePath);

            GameObject instance = Object.Instantiate(view);
            instance.transform.SetParent(container.transform);
            instance.transform.position = container.transform.position;
            
            return instance;
        }
    }
}