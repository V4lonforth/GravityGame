using System.Xml.Serialization;

namespace GravityGame.Levels.MapObjects
{
    [XmlRoot("GravityObject")]
    public class GravityInfo
    {
        [XmlElement("MapObject")]
        public MapObjectInfo MapObject;
        [XmlElement("GravityPower")]
        public float GravityPower;
    }
}