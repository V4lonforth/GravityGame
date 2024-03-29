﻿using System;
using Microsoft.Xna.Framework;

namespace GravityGame.GameObjects.MapObjects.Base
{
    public class MovingCircle : IMovingTrajectory
    {
        private Vector2 center;
        private float radius;
        private float angle;
        private float period;
        private float startTime;

        private const float PI2 = (float)(Math.PI * 2d);

        public MovingCircle(Vector2 center, float radius, float period, float startAngle = 0f, float startTime = 0f)
        {
            this.center = center;
            this.radius = radius;
            this.period = period;
            this.startTime = startTime;
            angle = startAngle;
        }

        public Vector2 GetPosition(float time)
        {
            angle = (time - startTime) / period * PI2;
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius + center;
        }
    }
}