using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using BepInEx;

namespace Destructy.Data.XML
{
    internal static class GameFoldersManager
    {
        internal static Dictionary<string, string> folders = new Dictionary<string, string>();
        internal static void CheckAndInit()
        {
            StreamReader stream = EmbeddedResourceManager.Get("Destructy.Data.XML.Folders.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            foreach (XmlNode node in doc.GetElementsByTagName("folder"))
            {
                string id = node.Attributes["id"].Value;
                string path = Path.Combine(Paths.GameRootPath, node.InnerText);
                folders.Add(id,  path);
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }


        }
        internal static string GetPathToFolder(string folderId)
        {
            return string.Empty;
        }
    }
}
