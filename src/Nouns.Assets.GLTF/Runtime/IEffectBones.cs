using Microsoft.Xna.Framework;

namespace Nouns.Assets.GLTF.Runtime;

public interface IEffectBones // it could be great if SkinnedEffect implemented this.
{
    void SetBoneTransforms(Matrix[] boneTransforms, int boneStart, int boneCount);
}