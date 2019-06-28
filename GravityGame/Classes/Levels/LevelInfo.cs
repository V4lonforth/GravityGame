using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using GravityGame.Levels.MapObjects;

namespace GravityGame.Levels
{
    [XmlRoot("Level")]
    public class LevelInfo
    {
        [XmlElement("PlayerPosition")]
        public Vector2 PlayerPosition;

        [XmlElement("Finish")]
        public MapObjectInfo Finish;

        [XmlElement("GravityObject")]
        public GravityInfo[] GravityObjects;

        [XmlElement("Portals")]
        public PortalsInfo[] Portals;

        //[XmlElement("PolygonObject")]
        //public PolygonObjectInfo[] PolygonObjects;
        //
        //[XmlElement("Portal")]
        //public PortalInfo[] Portals;
        //
        //[XmlElement("Charge")]
        //public int[] availableCharges;
    }
}