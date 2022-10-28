using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NGE.Engine;

public abstract class DrawContext
{
    public readonly SpriteBatch sb;
    public readonly SpriteFont defaultFont;
    public readonly Texture2D whitePixel;

    private readonly RasterizerState rasterizer;
    private readonly Effect effect;

    protected DrawContext(SpriteBatch sb, SpriteFont defaultFont, Texture2D whitePixel)
    {
        this.sb = sb;
        this.defaultFont = defaultFont;
        this.whitePixel = whitePixel;

        effect = new BasicEffect(sb.GraphicsDevice);
        rasterizer = new RasterizerState
        {
            CullMode = CullMode.CullCounterClockwiseFace
        };
    }

    public void Begin()
    {
        sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, rasterizer, effect);
    }

    public void BeginNoTransform()
    {
        sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
    }

    public void End()
    {
        sb.End();
    }
}