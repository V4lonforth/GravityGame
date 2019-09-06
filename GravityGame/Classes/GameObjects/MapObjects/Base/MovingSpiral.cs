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
        private float startRadius;
        private float startAngle;
        private float radiusSpeed;
        private float angleSpeed;
        private float startTime;

        public MovingSpiral(float startRadius, float startAngle, float radiusSpeed, float angleSpeed, float startTime)
        {
            this.startRadius = startRadius;
            this.startAngle = startAngle;
            this.radiusSpeed = radiusSpeed;
            this.angleSpeed = angleSpeed;
            this.startTime = startTime;
        }

        public MovingSpiral(MapObject center, float startRadius, float startAngle, float radiusSpeed, float angleSpeed, float startTime) 
            : this(startRadius, startAngle, radiusSpeed, angleSpeed, startTime)
        {
            centerObject = center;
        }
        public MovingSpiral(Vector2 center, float startRadius, float startAngle, float radiusSpeed, float angleSpeed, float startTime) 
            : this(startRadius, startAngle, radiusSpeed, angleSpeed, startTime)
        {
            centerStatic= center;
        }

        public Vector2 GetPosition(float time)
        {
            Vector2 center = centerObject == null ? centerStatic : centerObject.Position;
            float currentTime = time - startTime;
            float angle = startAngle + angleSpeed * currentTime;
            float radius = startRadius - radiusSpeed * currentTime;
            Vector2 localPosition = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
            return radius >= 0f ? center + localPosition : center;
        }
    }
}