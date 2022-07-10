namespace Nouns.Assets.Core
{
	public interface IHasReferencedAssets
	{
		IEnumerable<object> GetReferencedAssets();
        void ReplaceAsset(object search, object replace);
	}
}