using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nouns;

internal sealed class LoadingScreen
{
    private readonly Texture2D texture;
    private readonly NounsGame game;
    private readonly Color backgroundColor = new(233, 200, 11);

    public LoadingScreen(NounsGame game)
    {
        texture = Texture2D.FromStream(game.GraphicsDevice, File.OpenRead("Content\\logo.png"));
        this.game = game;
    }
    
    public void Draw()
    {
        game.GraphicsDevice.Clear(backgroundColor);

        var viewport = game.GraphicsDevice.Viewport;

        var position = new Vector2(
            viewport.Bounds.Width / 2f - texture.Width / 2f, 
            viewport.Bounds.Height / 2f - texture.Height / 2f);

        game.spriteBatch.Begin(0, null, SamplerState.PointClamp, null, null, null);
        game.spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.One, 1f, 0, 0);
        game.spriteBatch.End();
    }
}