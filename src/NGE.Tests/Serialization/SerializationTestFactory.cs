using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NGE.Engine.Pixel3D;
using NGE.Engine.Pixels;
using NGE.Graphics;

namespace NGE.Tests.Serialization;

public static class SerializationTestFactory
{
    public static AnimationSet CreateFakeAnimationSet()
    {
        var graphicsDevice = Headless.AcquireGraphicsDevice();
        var texture = new Texture2D(graphicsDevice, 1, 1);
        texture.SetData(new[] { Color.White });

        var sprite = new Sprite(texture);
        var cel = new Cel(sprite);
        var frame = new AnimationFrame();
        frame.layers.Add(cel);

        var animation = new Animation();
        animation.frames.Add(frame);

        var animationSet = new AnimationSet { friendlyName = "FakeAnimationSet" };
        animationSet.animations.Add(animation);

        return animationSet;
    }

    public static Level CreateFakeLevel()
    {
        var animationSet = CreateFakeAnimationSet();
        var level = new Level {friendlyName = "FakeLevel"};
        level.levelObjects.Add(new LevelObject(animationSet, new Position(100, 100), false));
        return level;
    }
}