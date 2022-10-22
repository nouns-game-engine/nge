namespace NGE.Engine.Pixel2D
{
    public static class Interpolation
    {
        public static float Clamp(this float value) =>
            value switch
            {
                < 0f => 0f,
                > 1f => 1f,
                _ => value
            };

        public static float MapFrom(this float value, float min, float max) => value - min / max - min;
    }
}
