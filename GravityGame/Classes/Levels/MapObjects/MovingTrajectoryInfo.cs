using System;
using GravityGame.GameObjects.MapObjects.Base;
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

        public IMovingTrajectory GetMovingTrajectory()
        {
            IMovingTrajectory movingTrajectory = null;
            switch (MovingType)
            {
                case MovingType.Circle:
                    movingTrajectory = new MovingCircle(Center, Radius, CirclePeriod, CircleAngle);
                    break;
                case MovingType.Linear:
                    movingTrajectory = new MovingLine(FirstPosition, SecondPosition, LinearPeriod, LinearTime);
                    break;
                case MovingType.Static:
                    movingTrajectory = new MovingStatic(Position);
                    break;
            }
            return movingTrajectory;
        }
    }
}