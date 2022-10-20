using Microsoft.Xna.Framework;

namespace NGE.Graphics;

public sealed class NoGame : Game
{
    // ReSharper disable once NotAccessedField.Local
    private readonly GraphicsDeviceManager graphics;
    public NoGame() => graphics = new GraphicsDeviceManager(this);
    protected override void Draw(GameTime gameTime) { }
}