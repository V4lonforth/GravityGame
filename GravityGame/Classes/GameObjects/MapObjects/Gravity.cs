﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GravityGame.GameObjects.Base;
using GravityGame.GameObjects.MapObjects.Base;
using GravityGame.Effects.GravityEffects;
using GravityGame.Utils;

namespace GravityGame.GameObjects.MapObjects
{
    public class Gravity : MapObject, IGameObject
    {
        private float gravityPower;

        private GravityParticlesDrawer particlesDrawer;

        private static Texture2D defaultSprite;

        private const float forceKoeff = 500000f;

        public Gravity(float gravityPower, IMovingTrajectory movingTrajectory, Vector2 size, float rotation, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0.5F) 
            : base(movingTrajectory, defaultSprite, size, rotation, Color.Black, spriteEffects, depth)
        {
            this.gravityPower = gravityPower;
            particlesDrawer = new GravityParticlesDrawer(this);
        }

        public static void LoadContent(ContentManager content)
        {
            defaultSprite = content.Load<Texture2D>("GameObjects/Circle");
        }

        public Vector2 CalculateForce(Vector2 position)
        {
            Vector2 offset = Position - position;
            float distance = offset.Length();
            Vector2 direction = offset / distance;
            return direction * (gravityPower / (distance * distance) * forceKoeff);
        }

        public void UpdateEffects(Time time)
        {
            particlesDrawer.Update(time);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            particlesDrawer.Draw();
            base.Draw(spriteBatch);
        }
    }
}