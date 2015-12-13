using UnityEngine;

namespace Commons.UI
{
    public interface IUIManager
    {
        TViewClass Show<TViewClass>(UIMap.Id _viewId);
        GameObject Show(UIMap.Id _viewId);
        void Hide(UIMap.Id _viewId);
    }
}