using Microsoft.Xna.Framework;

namespace GravityGame.Extensions
{
    public static class Functions
    {
        public static float Interpolate(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
        public static Vector2 Interpolate(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(Interpolate(a.X, b.X, t), Interpolate(a.Y, b.Y, t));
        }
    }
}