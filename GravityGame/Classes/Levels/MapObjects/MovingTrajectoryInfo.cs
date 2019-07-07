using System;
using Microsoft.Xna.Framework;

namespace GravityGame.Levels.MapObjects
{
    [Serializable]
    public class MovingTrajectoryInfo
    {
        public MovingType MovingType;

        public Vector2 Position;

        public Vector2 FirstPosition;
        public Vector2 SecondPosition;
        public float LinearPeriod;
        public float LinearTime;

        public Vector2 Center;
        public float Radius;
        public float CirclePeriod;
        public float CircleAngle;
    }
}