﻿using NGE.Assets;
using NGE.Engine.Pixel3D.Serialization;
using NGE.Engine.Pixels;
using NGE.Engine.Pixels.Serialization;

namespace NGE.Engine.Pixel3D.Assets;

// ReSharper disable once UnusedMember.Global (Reflection)
public sealed class AnimationSetReader : DeserializeAssetReader<AnimationSet, AnimationDeserializeContext>
{
    public AnimationSetReader() : base(".as") { }
}