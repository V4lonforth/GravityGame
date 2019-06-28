using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace GravityGame.Levels.MapObjects
{
    [XmlRoot("Transform")]
    public class Transform
    {
        [XmlElement("Position")]
        public Vector2 Position;
        [XmlElement("Size")]
        public float Size;
    }
}