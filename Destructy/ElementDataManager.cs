using System.Collections.Generic;
using System.IO;
using System.Xml;
using BepInEx;
using Destructy;
using Destructy.Data.XML;
namespace Destructy.Data
{
    internal static class ElementDataManager
    {
        internal static Dictionary<string, ElementData> elementData = new Dictionary<string, ElementData>();
        internal static void Load(string path)
        {
            if (!File.Exists(path))
            {
                Logging.LogError($"Element data file not found: {path}");
                return;
            }
            using (StreamReader reader = new StreamReader(path))
            {
                Load(reader);
            }
        }
        internal static void Load(StreamReader reader)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
        }

        internal static void LoadAllElementData()
        {
            
            

        }
    }
}