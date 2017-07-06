using UnityEngine;
using System.Linq;

namespace Commons.Utils
{
    public class InstantiateUtil
    {
        public static GameObject InstantiateAt(GameObject child, GameObject _parent, bool setPosition = false)
        {
            var instance = Object.Instantiate(child) as GameObject;
            instance.transform.parent = _parent.transform;
            if (setPosition)
                instance.transform.position = _parent.transform.position;
            return instance;
        }

        public static GameObject InstantiateUIAt(GameObject child, GameObject _parent)
        {
            var instance = Object.Instantiate(child) as GameObject;
            instance.transform.SetParent(_parent.transform, false);
            instance.transform.localScale = child.transform.localScale;

            return instance;
        }

        public static TComponent InstantiateAt<TComponent>(GameObject child, GameObject _parent, bool worldPositionStays = true) where TComponent: Component
        {
            var instance = Object.Instantiate(child) as GameObject;
            instance.transform.SetParent(_parent.transform, worldPositionStays);
            instance.transform.localScale = child.transform.localScale;
            return instance.GetComponent<TComponent>();
        }

        public static void AddToPath(string _path, GameObject _target)
        {
            var path = _path.Split('.');
            var go = GameObject.Find(path[0]);
            if (go == null)
                go = new GameObject(path[0]);
            for (var i = 1; i < path.Length; i++) {
                var finded = go.transform.Find(path[i]);
                GameObject current;
                if (finded == null) {
                    current = new GameObject(path[i]);
                    current.transform.parent = go.transform;
                } else
                    current = finded.gameObject;
                go = current;
            }

            _target.transform.parent = go.transform;
        }

        public static TComponent AddToPath<TComponent> (string _path) where TComponent: Component
        {
            var go = new GameObject(typeof(TComponent).Name);
            AddToPath(_path, go);
            return go.AddComponent<TComponent>();
        }
    }
}