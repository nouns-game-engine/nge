﻿using System.Collections.Generic;

namespace NGE.Assets
{
	public interface IHasReferencedAssets
	{
		IEnumerable<object> GetReferencedAssets();
        void ReplaceAsset(object search, object replace);
	}
}