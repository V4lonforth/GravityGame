using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace GravityGame.Levels.MapObjects
{
    [XmlRoot("Trajectory")]
    public class MovingTrajectoryInfo
    {
        [XmlElement("Type")]
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