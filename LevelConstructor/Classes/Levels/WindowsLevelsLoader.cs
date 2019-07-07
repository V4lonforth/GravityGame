using System;
using System.IO;
using System.Xml.Serialization;

namespace LevelConstructor.Levels
{
    public class WindowsLevelsLoader
    {
        const string infoPath = "Content/Levels/";

        public T LoadInfo<T>()
        {
            using (StreamReader stream = new StreamReader(GetFullPath()))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                T tInfo = (T)xmlSerializer.Deserialize(stream);
                return tInfo;
            }
        }

        public void Save<T>(T data)
        {
            using (StreamWriter stream = new StreamWriter(GetFullPath()))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stream, data);
            }
        }

        public static string GetFullPath()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;
            return projectDirectory + "/" + infoPath + "TestLevel" + ".xml";
        }

        public static DateTime GetFileChangedTime(string fileName)
        {
            FileInfo fileInfo = new FileInfo(GetFullPath());
            return fileInfo.LastWriteTime;
        }
    }
}
