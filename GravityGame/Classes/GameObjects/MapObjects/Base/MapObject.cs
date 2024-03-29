﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GravityGame.GameObjects.Base;
using GravityGame.Utils;

namespace GravityGame.GameObjects.MapObjects.Base
{
    public class MapObject : Drawable
    {
        private IMovingTrajectory movingTrajectory;

        public MapObject(IMovingTrajectory movingTrajectory, Texture2D sprite, Vector2 size, float rotation, Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0.5F) 
            : base(sprite, movingTrajectory.GetPosition(0f), size, rotation, color, spriteEffects, depth)
        {
            this.movingTrajectory = movingTrajectory;
        }

        public void Update(Time time)
        {
            Position = movingTrajectory.GetPosition(time.CurrentTime);
        }
    }
}