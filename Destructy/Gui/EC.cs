using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

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
            if(elements == null)
            {
                GenerateElements();
            }
            return elements;
        }
        internal Collector()
        {
            GenerateElements();
        }
    }
    internal class StatusCollector : Collector
    {
        internal override void GenerateElements()
        {
            elements = new Dictionary<string, Element>();
            elements.Add("godmode", new BoolElement("God Mode"));
            elements.Add("invisibility", new BoolElement("Invisibility"));
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
        }
    }
    internal class VehicleCollector : Collector
    {
        internal override void GenerateElements()
        {
            elements = new Dictionary<string, Element>();
            elements.Add("free_aim", new BoolElement("Free Aim"));
            elements.Add("rapid_fire", new BoolElement("Rapid Fire"));
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
        }
    }
}
