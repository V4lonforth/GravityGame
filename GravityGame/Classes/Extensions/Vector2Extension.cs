using System;
using GravityGame.Utils;

namespace Microsoft.Xna.Framework
{
    public static class Vector2Extension
    {
        public static Vector2 Rotate(this Vector2 vec1, Vector2 vec2)
        {
            Vector2 vector;
            vector.X = vec1.X * vec2.X - vec1.Y * vec2.Y;
            vector.Y = vec1.Y * vec2.X + vec1.X * vec2.Y;
            return vector;
        }
        public static Vector2 Rotate(this Vector2 vec, float angle)
        {
            Vector2 vector;
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            vector.X = vec.X * cos - vec.Y * sin;
            vector.Y = vec.Y * cos + vec.X * sin;
            return vector;
        }

        public static Vector2 ScreenToGUIPosition(this Vector2 position)
        {
            position -= Screen.ScreenSize.ToVector2() / 2f;
            return position / Screen.Size;
        }

        public static Vector2 ScreenToWorldPosition(this Vector2 position)
        {
            position -= Screen.ScreenSize.ToVector2() / 2f;
            return position / Screen.Size + Screen.ScreenPosition;
        }

        public static float DotProduct(this Vector2 vector1, Vector2 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        public static Vector2 Normalized(this Vector2 vector, out float length)
        {
            length = vector.Length();
            return vector / length;
        }
    }
}