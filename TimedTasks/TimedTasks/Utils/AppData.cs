using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;

namespace TimedTasks.Utils
{
    [Serializable]
    [XmlRoot]
    public class AppData
    {
        [XmlElement]
        public string TaskSelectOption { get; set; }

        [XmlElement]
        public SerializableDictionary<string, bool> GroupVisibilitySetting { get; set; } = new SerializableDictionary<string, bool>();

        public AppData()
        {
        }

        [XmlIgnore]
        public static AppData Data;

        [XmlIgnore]
        private const string fileName = "AppData.xml";

        private static string GetFilePath()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filePath = Path.Combine(path, fileName);
            return filePath;
        }

        public static void LoadAppData()
        {
            var path = GetFilePath();
            if (!File.Exists(path))
            {
                Data = new AppData();
                return;
            }

            var xml = File.ReadAllText(path);
            Data = XML.Deserialize<AppData>(xml);
        }

        public static void SaveAppData()
        {
            if (Data == null)
                return;

            var path = GetFilePath();
            var xml = XML.Serialize(Data);
            File.WriteAllText(path, xml);
        }
    }

    public class LastSelectedDate
    {
        [XmlElement]
        public DateTime SelectionTime { get; set; }

        [XmlElement]
        public DateTime SelectedDate { get; set; }

        [XmlElement]
        public TimeSpan StartTime { get; set; }

        [XmlElement]
        public TimeSpan EndTime { get; set; }
    }
}
