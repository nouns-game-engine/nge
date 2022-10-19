﻿using Microsoft.Xna.Framework;

namespace Nouns.Graphics.Core;

public sealed class NoGame : Game
{
    // ReSharper disable once NotAccessedField.Local
    private readonly GraphicsDeviceManager graphics;
    public NoGame() => graphics = new GraphicsDeviceManager(this);
    protected override void Draw(GameTime gameTime) { }
}