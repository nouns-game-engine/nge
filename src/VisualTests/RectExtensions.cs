using Microsoft.Xna.Framework;
using Nouns.Pipeline;

namespace VisualTests
{
    public static class RectExtensions
    {
        public static Rectangle ToRectangle(this Rect rect)
        {
            return new Rectangle(rect.x, rect.y, rect.w, rect.h);
        }
    }
}
