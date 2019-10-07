namespace Microsoft.Xna.Framework
{
    public static class RectangleExtension
    {
        public static Vector2 Clamp(this Rectangle rect, Vector2 pos)
        {
            if (pos.X < rect.Left)
                pos.X = rect.Left;
            else if (pos.X > rect.Right)
                pos.X = rect.Right;

            if (pos.Y < rect.Top)
                pos.Y = rect.Top;
            else if (pos.Y > rect.Bottom)
                pos.Y = rect.Bottom;

            return pos;
        }
    }
}