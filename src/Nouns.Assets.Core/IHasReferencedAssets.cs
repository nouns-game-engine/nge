using System.Collections.Generic;

namespace Nouns.Assets
{
	public interface IHasReferencedAssets
	{
		IEnumerable<object> GetReferencedAssets();
        void ReplaceAsset(object search, object replace);
	}
}