using System.Xml.Serialization;

namespace GravityGame.Levels.MapObjects
{
    [XmlRoot("Portals")]
    public class PortalsInfo
    {
        [XmlElement("First")]
        public MapObjectInfo FirstPortal;
        [XmlElement("Second")]
        public MapObjectInfo SecondPortal;
    }
}