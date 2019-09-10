using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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

        public MovingSpiral(Vector2 center, Vector2 firstPosition, Vector2 secondPosition, Vector2 velocity, float startTime)
        {
            this.startTime = startTime;

            Vector2 normal = secondPosition - firstPosition;
            normal = new Vector2(-normal.Y, normal.X).Normalized(out float length);

            Vector2 localPosition = firstPosition - center;
            Vector2 localPerpendicular = Vector2.Dot(normal, firstPosition - center) * normal;

            float r1 = localPosition.Length();
            float f1 = (float)Math.Atan2(localPosition.Y, localPosition.X);
            startRadius = localPerpendicular.Length();
            startAngle = (float)Math.Atan2(localPerpendicular.Y, localPerpendicular.X);

            a = (float)Math.Pow(r1 / startRadius, 1f / (f1 - startAngle));
            b = (float)Math.Log(r1, a) - f1;

            speed = (float)Math.Log(Math.Pow(a, startAngle) + velocity.Length() / (Math.Pow(a, b) * Math.Sqrt(1 + 1 / Math.Pow(Math.Log(a), 2))), a);
        }

        public MovingSpiral(MapObject center, Vector2 firstPosition, Vector2 secondPosition, Vector2 velocity, float startTime) 
            : this(center.Position, firstPosition, secondPosition, velocity, startTime)
        {
            centerObject = center;
        }

        public Vector2 GetPosition(float time)
        {
            Vector2 center = centerObject == null ? centerStatic : centerObject.Position;
            float currentTime = time - startTime;

            float angle = -currentTime * speed + startAngle;
            float radius = (float)Math.Pow(a, b + angle);
            Vector2 localPosition = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;

            Ended = radius <= 0f;
            Size = radius / startRadius;
            return radius >= 0f ? center + localPosition : center;
        }
    }
}