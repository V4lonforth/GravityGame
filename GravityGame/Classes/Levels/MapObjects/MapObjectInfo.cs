using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace GravityGame.Levels.MapObjects
{
    [XmlRoot("Drawable")]
    public class MapObjectInfo
    {
        [XmlElement("Size")]
        public Vector2 Size;
        [XmlElement("Rotation")]
        public float Rotation;
        [XmlElement("Trajectory")]
        public MovingTrajectoryInfo Trajectory;
        [XmlElement("Color")]
        public Color Color;
    }
}