using System;
using Microsoft.Xna.Framework;

namespace GravityGame.GameObjects.MapObjects.Base
{
    public class MovingSpiral : IMovingTrajectory
    {
        private MapObject centerObject;
        private Vector2 centerStatic;
        private float startTime;

        private float startAngle;
        private float startRadius;
        private float a;
        private float b;
        private float speed;

        public float Size { get; private set; }
        public bool Ended { get; private set; }

        private const float minDeltaAngle = 0.001f;

        public MovingSpiral(Vector2 center, Vector2 position, Vector2 velocity, float startTime)
        {
            this.startTime = startTime;
            centerStatic = center;

            startAngle = (float)Math.Atan2(position.Y, position.X);
            startRadius = position.Length();

            position = position - center + velocity.Normalized(out float length);

            float deltaAngle = (float)Math.Atan2(position.Y, position.X) - startAngle;
            float deltaRadius = position.Length() - startRadius;

            if (deltaRadius > 1f)
                deltaRadius = 1f;

            if (deltaAngle >= 0f)
            {
                if (deltaAngle < minDeltaAngle)
                    deltaAngle = minDeltaAngle;
            }
            else if (deltaAngle > -minDeltaAngle)
                deltaAngle = -minDeltaAngle;

            a = (float)Math.Exp(deltaRadius / (deltaAngle * startRadius));
            b = (float)Math.Log(startRadius, a) - startAngle;
            speed = deltaAngle * length;
            //speed = -(float)Math.Log(Math.Pow(a, startAngle) + velocity.Length() / (Math.Pow(a, b) * Math.Sqrt(1 + 1 / Math.Pow(Math.Log(a), 2))), a);
            //if (a > 0 && a < 1 && speed < 0)
            //    if (speed < -1)
            //        speed *= -1;
            //    else
            //        speed = -1 / speed;
        }

        public MovingSpiral(MapObject center, Vector2 position, Vector2 velocity, float startTime)
            : this(center.Position, position, velocity, startTime)
        {
            centerObject = center;
        }

        public Vector2 GetPosition(float time)
        {
            Vector2 center = centerObject == null ? centerStatic : centerObject.Position;
            float currentTime = time - startTime;

            float angle = currentTime * speed + startAngle;
            float radius = (float)Math.Pow(a, angle + b);
            Vector2 localPosition = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;

            Ended = radius <= 0f;
            Size = (float)Math.Sqrt(radius / startRadius);
            return radius >= 0f ? center + localPosition : center;
        }
    }
}