namespace Commons.Resources
{
	public interface IResourceManager
	{
		TType GetResource<TType>(string resourcePath);
	}
}