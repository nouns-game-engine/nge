using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Velentr.Font;

namespace NGE;

internal sealed class LoadingScreen
{
    private readonly Texture2D texture;
    private readonly NounsGame game;
    private readonly Color backgroundColor = new(147, 215, 225);

    public LoadingScreen(NounsGame game)
    {
        texture = Texture2D.FromStream(game.GraphicsDevice, File.OpenRead("Content\\logo.png"));
        this.game = game;
    }

    private Font? screenFont;

    public void Update()
    {
        screenFont ??= game.fontManager.GetFont("./Content/LondrinaSolid-Regular.ttf", 48);
    }

    public void Draw(GameTime gameTime)
    {
        if (screenFont == null)
            return;

        game.GraphicsDevice.Clear(backgroundColor);

        game.sb.Begin(0, null, SamplerState.PointClamp, null, null, null);

        var viewport = game.GraphicsDevice.Viewport;

        {
            var position = new Vector2(
                viewport.Bounds.Width / 2f - texture.Width / 2f,
                viewport.Bounds.Height / 2f - texture.Height / 2f);

            game.sb.Draw(texture, position, null, Color.White, 0f, Vector2.One, 1f, 0, 0);
        }

        {
            var text = game.RunningFor.TotalSeconds.ToString(CultureInfo.CurrentCulture);
            var position = new Vector2(viewport.Bounds.Left + 10, viewport.Bounds.Bottom - screenFont!.GlyphHeight - 3);
            var color = gameTime.IsRunningSlowly ? Color.Red : Color.White;

            game.sb.DrawString(screenFont, text, position, color);
        }

        game.sb.End();
    }
}