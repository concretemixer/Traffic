namespace Commons.Resources.Local
{
    public class LocalResourceManager : IResourceManager
    {
        public void Init(string _basePath)
        {

        }

        public TType GetResource<TType>(string resourcePath) where TType : UnityEngine.Object
        {
            var resource = UnityEngine.Resources.Load(resourcePath);
            if (resource == null)
                UnityEngine.Resources.Load(resourcePath + ".prefab");
            return (TType)resource;
        }
    }
}