using System.Collections.Generic;
using UnityEngine;

namespace Destructy.Gui.FuncLists.Elements
{
    internal class Element
    {
        internal virtual string GetRenderText()
        {
            return "NULL";
        }
        internal virtual ElementData CollectData()
        {
            return new ElementData("NULL", new List<KeyCode>(), new Dictionary<string, object>());
        }
    }
}
