using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityGame.GameObjects.Base
{
    public class WrappedDrawable : Drawable
    {
        public float StartWidth { get; set; }

        public WrappedDrawable(Texture2D sprite, Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0.5F) 
            : base(sprite, color, spriteEffects, depth)
        {
        }
        public WrappedDrawable(Texture2D sprite, Vector2 position, Vector2 size, float rotation, Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0.5F) 
            : base(sprite, position, size, rotation, color, spriteEffects, depth)
        {
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Position, new Rectangle(new Point((int)StartWidth, 0), Size.ToPoint()), Color, Rotation, Size / 2f, 1f, spriteEffects, depth);
        }
    }
}