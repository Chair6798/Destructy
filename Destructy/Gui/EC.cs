using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destructy
{
    internal static class ElementCollector
    {
        internal static Dictionary<string, Collector> collectors;
        internal static void Init()
        {
            collectors = new Dictionary<string, Collector>();
            collectors.Add("You", new YouCollector());
            collectors.Add("Visuals", new VisualsCollector());
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
    }
    internal class YouCollector : Collector
    {
        internal override void GenerateElements()
        {
            elements = new Dictionary<string, Element>();
            elements.Add("GodMode", new BoolElement("GodMode"));
            elements.Add("Noclip", new BoolElement("Noclip"));
            elements.Add("InfiniteAmmo", new BoolElement("InfiniteAmmo"));
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
}
