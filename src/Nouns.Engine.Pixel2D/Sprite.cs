using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Editor;

namespace Nouns.Engine.Pixel2D;

public struct Sprite
{
    [NonEditable]
    public Texture2D texture;

    public Rectangle sourceRectangle;
    public Point origin;

    public Sprite(Texture2D texture) : this(texture, texture.Bounds, new Point(0, texture.Height - 1)) { }
    public Sprite(Texture2D texture, Rectangle sourceRectangle) : this(texture, sourceRectangle, new Point(0, texture.Height - 1)) { }
    public Sprite(Texture2D texture, Point origin) : this(texture, texture.Bounds, origin) { }

    public Sprite(Texture2D texture, Rectangle sourceRectangle, Point origin)
    {
        this.texture = texture;
        this.sourceRectangle = sourceRectangle;
        this.origin = origin;
    }

    public Vector2 DrawOrigin => new(origin.X, origin.Y + 1);
}