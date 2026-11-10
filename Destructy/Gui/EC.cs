using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

namespace Destructy
{
    internal static class ElementCollector
    {
        internal static Dictionary<string, Collector> collectors = new Dictionary<string, Collector>();
        internal static void Init()
        {
            collectors = new Dictionary<string, Collector>();
            collectors.Add("Status", new StatusCollector());
            collectors.Add("Movement", new MovementCollector());
            collectors.Add("Weapon", new WeaponCollector());
            collectors.Add("Vehicle", new VehicleCollector());
            collectors.Add("Visuals", new VisualsCollector());
            collectors.Add("World", new WorldCollector());
            Load();
        }
        
        internal static Element GetElement(string collector, string element)
        {
            Collector collInst;
            if (!collectors.TryGetValue(collector, out collInst))
            {
                return null;
            }
            Element elementInst;
            if (!collInst.elements.TryGetValue(element, out elementInst))
            {
                return null;
            }
            return elementInst;
        }
        internal static UnityEvent GetEvent(string collector, string element)
        {
            Element elementInst = GetElement(collector, element);
            if (elementInst == null)
            {
                return null;
            }
            if (elementInst is FuncElement)
            {
                return ((FuncElement)elementInst).Event;
            }
            return null;
        }
        internal static bool GetBool(string collector, string element)
        {
            Element elementInst = GetElement(collector, element);
            if(elementInst==null)
            {
                return false;
            }
            if(elementInst is BoolElement)
            {
                return ((BoolElement)elementInst).value;
            }
            return false;
        }
        internal static string GetString(string collector, string element)
        {
            Element elementInst = GetElement(collector, element);
            if (elementInst == null)
            {
                return null;
            }
            if (elementInst is StringElement)
            {
                return ((StringElement)elementInst).value;
            }
            return null;
        }
        internal static int GetEnum(string collector, string element)
        {
            Element elementInst = GetElement(collector, element);
            if (elementInst == null)
            {
                return -1;
            }
            if (elementInst is EnumElement)
            {
                return ((EnumElement)elementInst).value;
            }
            return -1;
        }
        internal static float GetFloat(string collector, string element)
        {
            Element elementInst = GetElement(collector, element);
            if (elementInst == null)
            {
                return -1f;
            }
            if (elementInst is FloatElement)
            {
                return ((FloatElement)elementInst).value;
            }
            return -1f;
        }

        internal static void Save()
        {
            foreach(string name in collectors.Keys)
            {
                Collector c = collectors[name];
                XmlDocument doc = c.GetSave();
                string dirPath = Path.Combine(Paths.GameRootPath, "Destructy", "Collectors");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                doc.Save(Path.Combine(Paths.GameRootPath, "Destructy", "Collectors", name + ".xml"));
            }
        }
        internal static void Load()
        {
            foreach (string name in collectors.Keys)
            {
                string path = Path.Combine(Paths.GameRootPath, "Destructy", "Collectors", name + ".xml");
                if(File.Exists(path))
                {
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        doc.Load(path);
                        Logging.LogInfo("Loaded document. Putting data in collector...");
                        collectors[name].Load(doc);
                    }
                    catch(Exception e)
                    {
                        Logging.LogWarning($"Failed to load save for {name}. Error: {e.Message}");
                    }
                }
            }
        }


    }
    internal class Collector
    {
        internal Dictionary<string, Element> elements;
        internal virtual void GenerateElements()
        {
            Logging.Log("Base class of collector is used. Nothing happend.");
        }
        internal virtual Dictionary<string, Element> Get()
        {
            if (elements == null)
            {
                GenerateElements();
            }
            return elements;
        }
        internal Collector()
        {
            GenerateElements();
        }
        internal XmlDocument GetSave()
        {
            XmlDocument doc = new XmlDocument();
            var main = doc.AppendChild(doc.CreateElement("collector"));
            foreach (string id in elements.Keys)
            {
                Element element = elements[id];
                var xmlElement = doc.CreateElement("element");
                string saveValue = element.GetSaveValue();
                if (saveValue != string.Empty)
                    xmlElement.SetAttribute("value", saveValue);
                xmlElement.SetAttribute("bind", ((int)element.bind).ToString());
                xmlElement.SetAttribute("id", id);
                main.AppendChild(xmlElement);
            }
            return doc;
        }
        internal void Load(XmlDocument doc)
        {
            foreach (XmlElement element in doc.GetElementsByTagName("element"))
            {
                if (!element.HasAttribute("id"))
                    continue;
                string id = element.GetAttribute("id");
                Element elementInst;
                if (!elements.TryGetValue(id, out elementInst))
                {
                    continue;
                }
                KeyCode bind = KeyCode.None;
                if (element.HasAttribute("bind"))
                {
                    int result = -1;
                    string bindString = element.GetAttribute("bind");
                    if (int.TryParse(bindString, out result))
                    {
                        try
                        {
                            bind = (KeyCode)result;
                        }
                        catch
                        {
                            Logging.LogWarning("Failed to load bind, using None. KeyCode id: " + result.ToString());
                        }

                    }
                    else
                    {
                        Logging.LogWarning("Failed to load bind, using None. Cant parse KeyCode id. Text: " + bindString);
                    }
                }
                else
                {
                    Logging.LogWarning("Failed to load bind, using None. No bind attribute. Save corrupted?");
                }
                if (element.HasAttribute("value"))
                    elementInst.PutSaveValue(element.GetAttribute("value"));
                Logging.LogInfo($"Loaded {id}.");

            }

        }
    }
    internal class StatusCollector : Collector
    {
        internal override void GenerateElements()
        {
            elements = new Dictionary<string, Element>();
            elements.Add("godmode", new BoolElement("God Mode"));
            elements.Add("invisibility", new BoolElement("Invisibility"));
            elements.Add("change_nickname", new BoolElement("Change Nickname"));
            elements.Add("custom_nickname", new StringElement("Custom Nickname"));
            elements.Add("custom_message_mode", new EnumElement("Message style", new List<string> { "Off", "Edit", "Preset" }));
            elements.Add("custom_message_style", new StringElement("Message style"));
            elements.Add("fake_device", new EnumElement("Fake Device", new List<string> { "Disabled", "Empty", "Steam", "Android", "iOS" }));
            elements.Add("prevent_disconnect", new BoolElement("Prevent Disconnect"));
            //elements.Add("break_host", new BoolElement("Break Host"));
        }
    }
    internal class MovementCollector : Collector
    {
        internal override void GenerateElements()
        {
            elements = new Dictionary<string, Element>(); 
            elements.Add("noclip", new BoolElement("Noclip"));
        }
    }
    
    internal class WeaponCollector : Collector
    {
        internal override void GenerateElements()
        {
            elements = new Dictionary<string, Element>();
            elements.Add("infinite_ammo", new BoolElement("Infinite Ammo"));
            elements.Add("rapid_fire", new BoolElement("Rapid Fire"));
            elements.Add("no_recoil", new BoolElement("No Recoil"));
            elements.Add("one_hit", new BoolElement("One Hit"));
            //
            elements.Add("get_menu", new FuncElement("Get Menu"));
            elements.Add("get_minigun", new FuncElement("Get Minigun"));
        }
    }
    internal class VehicleCollector : Collector
    {
        internal override void GenerateElements()
        {
            elements = new Dictionary<string, Element>();
            elements.Add("free_aim", new BoolElement("Free Aim"));
            elements.Add("rapid_fire", new BoolElement("Rapid Fire"));
            elements.Add("scale", new FloatElement("Scale", 1, 0.001f, 20f, false));
            elements.Add("torque_modifier", new FloatElement("Torque", 1, 0.5f, 20f, false));
            elements.Add("no_speed_limit", new BoolElement("No Speed Limit"));
            elements.Add("toyotayaris", new BoolElement("Toyota Yaris"));
            //elements.Add("eject", new FuncElement("Eject"));
        }
    }
    internal class VisualsCollector : Collector
    {
        internal override void GenerateElements()
        {
            elements = new Dictionary<string, Element>();
            elements.Add("players_esp", new BoolElement("Players Esp"));
            elements.Add("players_box", new BoolElement("Players Box"));
            elements.Add("players_tracer", new BoolElement("Players Tracer"));
        }
    }
    internal class WorldCollector : Collector
    {
        internal override void GenerateElements()
        {
            elements = new Dictionary<string, Element>();
            elements.Add("rocket_launcher", new BoolElement("Rocket Launcher"));
            elements.Add("destruction_multiplier", new FloatElement("Destruction Multiplier", 1f, 0f, 15f));
            elements.Add("clean_map", new FuncElement("Clean map"));
            elements.Add("remove_fire", new FuncElement("Remove Fire"));
        }
    }
}
