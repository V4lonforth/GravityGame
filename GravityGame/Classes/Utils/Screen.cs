using Microsoft.Xna.Framework;
using Android.Util;

namespace GravityGame.Utils
{
    public static class Screen
    {
        public static Point ScreenSize;
        public static Rectangle ScreenRect;

        public const int SceneHeight = 2000;
        public static Point SceneSize;
        public static Rectangle SceneRect;

        public static float Size;
        public static Matrix GUIMatrix;
        public static Matrix SceneMatrix;

        public static Vector2 ScreenPosition;

        private const int BorderOffset = 300;

        public static void CreateData(DisplayMetrics displayMetrics)
        {
            ScreenSize.X = displayMetrics.WidthPixels;
            ScreenSize.Y = displayMetrics.HeightPixels;
            ScreenRect = new Rectangle(Point.Zero, ScreenSize);

            Size = ScreenSize.Y / (float)SceneHeight;
            SceneSize = new Point((int)(ScreenSize.X / Size), SceneHeight);
            SceneRect = new Rectangle(-SceneSize.X / 2, -SceneSize.Y / 2, SceneSize.X, SceneSize.Y);

            CreateGUIMatrix();
            CreateSceneMatrix();
        }
        private static void CreateGUIMatrix()
        {
            GUIMatrix = Matrix.CreateScale(Size) * Matrix.CreateTranslation(new Vector3(ScreenSize.X / 2f, ScreenSize.Y / 2f, 0f));
        }
        private static void CreateSceneMatrix()
        {
            SceneMatrix = Matrix.CreateScale(Size) * Matrix.CreateTranslation(new Vector3(ScreenSize.X / 2f, ScreenSize.Y / 2f, 0f));
        }

        public static void MoveCamera(Vector2 offset)
        {
            SceneMatrix = Matrix.CreateTranslation(offset.X, offset.Y, 0f) * SceneMatrix;
        }

        public static void SetCameraPosition(Vector2 position)
        {
            CreateSceneMatrix();
            SceneMatrix = Matrix.CreateTranslation(position.X, position.Y, 0f) * SceneMatrix;
        }
    }
}