using System;
using Microsoft.Xna.Framework;
using GravityGame.Extensions;

namespace GravityGame.GameObjects.MapObjects.Base
{
    public class MovingLine : IMovingTrajectory
    {
        private Vector2 position1;
        private Vector2 position2;
        private float period;
        private float time;

        public MovingLine(Vector2 position1, Vector2 position2, float period, float startTime = 0f)
        {
            this.position1 = position1;
            this.position2 = position2;
            this.period = period;
            time = startTime;
        }

        public Vector2 GetPosition(float deltaTime)
        {
            time += deltaTime;
            float pos = (float)((Math.Sin(time) + 1d) / 2d);
            return Functions.Interpolate(position1, position2, pos);
        }
    }
}