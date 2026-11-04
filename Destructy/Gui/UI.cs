using UnityEngine;
using System.Collections.Generic;
using Destructy;
using System.Xml.Serialization;
using System.CodeDom;
namespace Destructy.Gui
{
    internal class MainUIBehaviour : MonoBehaviour
    {
        static List<ElementList> lists = new List<ElementList>();
        internal static bool render = true;
        internal static float animation = 0f;
        void Awake()
        {
            ElementCollector.Init();
            foreach (var key in ElementCollector.collectors.Keys)
            {
                var coll = ElementCollector.collectors[key];
                var o = new ElementList(5 + coll.elements.Count * (30 + 5), coll.elements);
                o.title = key;
                lists.Add(o);
            }
        }
        void OnGUI()
        {
            int i = 0;
            while (i<lists.Count)
            {
                GUI.Window(i + 2, new Rect(10 + i * (200 + 10) + Screen.width*animation, 10, 200, lists[i].visual_height+20), DrawElementList, lists[i].title);
                i++;
            }

        }
        void Update()
        {
            if (animation > 0)
            {
                animation += Time.deltaTime * 2;
                if (animation > 1)
                {
                    animation = 1;
                    render = false;
                }
            }
            if(animation < 0)
            {
                animation += Time.deltaTime * 2;
                if (animation > 0)
                {
                    animation = 0;
                }
            }
            if (Input.GetKeyDown(Loader.Main.menuToggle.Value))
            {
                if (render&&animation==0)
                {
                    AnimateHide();
                }
                else
                {
                    AnimateShow();
                }
            }
        }
        internal static void AnimateHide()
        {
            animation = 0.01f;
        }
        internal static void AnimateShow()
        {
            animation = -1f;
            render = true;
        }
        static void DrawElementList(int wID)
        {
            lists[wID - 2].Draw();
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
            switch(element.GetType().Name)
            {
                case ("StringElement"):
                    ((StringElement)element).value = GUI.TextField(new Rect(5, 5 + index * (30 + 5), 190, 30), ((StringElement)element).value);
                    break;
                default:
                    if (GUI.Button(new Rect(5, 5 + index * (30 + 5), 190, 30), element.GetRenderText()))
                    {
                        element.Use();
                    }
                    break;
            }
            
        }
        internal virtual void DrawEnd()
        {
            GUI.EndScrollView();
        }
    }
}
