using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nouns;

internal sealed class NounsGame : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    
    public NounsGame()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
        graphics.PreferMultiSampling = true;
        Content.RootDirectory = "Content";

        Window.AllowUserResizing = true;
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        base.LoadContent();
    }

    protected override void UnloadContent()
    {
        Content.Unload();
        spriteBatch.Dispose();
        base.UnloadContent();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        base.Draw(gameTime);
    }
}