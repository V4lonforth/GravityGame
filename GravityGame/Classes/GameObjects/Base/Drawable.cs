using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GravityGame.GameObjects.Base
{
    public class Drawable : IGameObject
    {
        protected Texture2D sprite;

        protected SpriteEffects spriteEffects;
        protected float depth;

        public float Radius { get; protected set; }
        public Vector2 Position { get; set; }
        public Vector2 Center { get; protected set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public Vector2 Size { get; set; }

        protected Vector2 Scale
        {
            set
            {
                Size = new Vector2(value.X * sprite.Width, value.Y * sprite.Height);
            }
            get
            {
                return new Vector2(Size.X / sprite.Width, Size.Y / sprite.Height);
            }
        }

        public Drawable(Texture2D sprite, Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0.5f)
        {
            this.sprite = sprite;
            this.spriteEffects = spriteEffects;
            this.depth = depth;
            Color = color.HasValue ? color.Value : Color.White;
            if (sprite != null)
                Center = sprite.Bounds.Size.ToVector2() / 2f;
        }

        public Drawable(Texture2D sprite, Vector2 position, Vector2 size, float rotation, Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0.5f)
            : this(sprite, color, spriteEffects, depth)
        {
            Position = position;
            Size = size;
            Radius = size.X / 2f;
            Rotation = rotation;
        }

        public bool CheckCollision(IGameObject drawable)
        {
            return (drawable.Radius + Radius) * (drawable.Radius + Radius) >= (Position - drawable.Position).LengthSquared();
        }
        public bool CheckCollision(List<IGameObject> drawables)
        {
            foreach (IGameObject gameObject in drawables)
                if (CheckCollision(gameObject))
                    return true;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (sprite != null)
                spriteBatch.Draw(sprite, Position, null, Color, Rotation, Center, Scale, spriteEffects, depth);
        }

        public void Update()
        {
        }
    }
}