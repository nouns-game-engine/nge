namespace Nouns.Assets.Core
{
	public interface IAssetPathProvider
	{
		string? GetAssetPath<T>(T asset) where T : class;
	}
}