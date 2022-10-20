using System.Collections.Generic;

namespace NGE.Assets
{
	public interface IAssetProvider
	{
		T? Load<T>(string assetPath) where T : class;
		ICollection<T> LoadAll<T>() where T : class;
	}
}