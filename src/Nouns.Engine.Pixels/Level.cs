using Nouns.Assets.Core;

namespace Nouns.Engine.Pixels;

public class Level : IHasReferencedAssets
{
    public readonly Dictionary<string, string> properties = new();

    public readonly List<Thing> things;

    public Level()
    {
        things = new List<Thing>();
    }

    #region IHasReferencedAssets Members

    public IEnumerable<object> GetReferencedAssets()
    {
        foreach (var thing in things)
            yield return thing.AnimationSet;
    }

    public void ReplaceAsset(object search, object replace)
    {
        if (search is not AnimationSet)
            return;

        foreach (var thing in things)
        {
            if (!ReferenceEquals(thing.AnimationSet, search))
                continue;

            thing.AnimationSet = (AnimationSet) replace;
        }
    }

    #endregion
}
