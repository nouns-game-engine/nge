namespace NGE.Assets
{
	public interface IAssetPathProvider
	{
		string? GetAssetPath<T>(T asset) where T : class;
	}
}