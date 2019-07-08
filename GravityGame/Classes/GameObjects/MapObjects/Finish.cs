using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GravityGame.GameObjects.MapObjects.Base;

namespace GravityGame.GameObjects.MapObjects
{
    public class Finish : MapObject
    {
        private static Texture2D defaultSprite;
        private static Color defaultColor = new Color(115, 221, 126);

        public Finish(IMovingTrajectory movingTrajectory, Vector2 size, float rotation, Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0.5F) 
            : base(movingTrajectory, defaultSprite, size, rotation, color == new Color(0, 0, 0, 0) ? defaultColor : color, spriteEffects, depth)
        {
        }

        public static void LoadContent(ContentManager content)
        {
            defaultSprite = content.Load<Texture2D>("GameObjects/Circle");
        }
    }
}