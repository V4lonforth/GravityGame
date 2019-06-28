using System.IO;
using System.Xml.Serialization;

namespace GravityGame.Levels
{
    public class LevelsLoader
    {
        const string infoPath = "Content/Levels/";

        public T LoadInfo<T>(string fileName)
        {
            using (Stream stream = Microsoft.Xna.Framework.Game.Activity.Assets.Open(infoPath + fileName + ".xml"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                T tInfo = (T)xmlSerializer.Deserialize(stream);
                return tInfo;
            }
        }

        public void Save<T>(string fileName, T data)
        {
            using (StreamWriter stream = new StreamWriter(infoPath + fileName + ".xml"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stream, data);
            }
        }
    }
}