using UnityEngine;
using System.Collections.Generic;
using Destructy;
using System.Xml.Serialization;
namespace Destructy.Gui
{
    internal class MainUIBehaviour : MonoBehaviour
    {
        static List<ElementList> lists = new List<ElementList>();
        void Awake()
        {
            ElementCollector.Init();
            foreach (var key in ElementCollector.collectors.Keys)
            {
                var coll = ElementCollector.collectors[key];
                var o = new ElementList(20 + 5 + coll.elements.Count * (30 + 5), coll.elements);
                o.title = key;
                lists.Add(o);
            }
        }
        void OnGUI()
        {
            int i = 0;
            while (i<lists.Count)
            {
                GUI.Window(i + 1, new Rect(10 + i * (200 + 10), 10, 200, lists[i].visual_height+20), DrawElementList, lists[i].title);
                i++;
            }

        }
        static void DrawElementList(int wID)
        {
            lists[wID - 1].Draw();
        }
    }
    internal class ElementList
    {
        internal ElementList(float height, Dictionary<string,Element> dictionary)
        {
            visual_height = height;
            this.dictionary = dictionary;
        }

        internal Vector2 scrollPos = Vector2.zero;
        internal string title = "NONE";
        internal Dictionary<string, Element> dictionary;
        internal float visual_height = 300;
        internal float scroll_height = 300;
        internal bool scroll_height_equal = true;
        //internal float horizontal_index = 0;
        internal virtual void Draw()
        {
            DrawBegin();
            DrawElements();
            DrawEnd();
        }
        internal virtual void DrawBegin()
        {
            scrollPos = GUI.BeginScrollView(new Rect(0, 15, 200, scroll_height_equal?visual_height:scroll_height), scrollPos, new Rect(0,0,200,visual_height), false, false);
        }
        internal virtual void DrawElements()
        {
            int index = 0;
            foreach(Element element in dictionary.Values)
            {
                DrawElement(element, index);
                index++;
            }
        }
        internal virtual void DrawElement(Element element, int index)
        {
            if(GUI.Button(new Rect(5, 5 + index*(30+5), 190, 30), element.GetRenderText()))
            {
                element.Use();
            }
        }
        internal virtual void DrawEnd()
        {
            GUI.EndScrollView();
        }
    }
}
