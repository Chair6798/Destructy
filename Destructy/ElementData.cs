using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Destructy
{
    internal class ElementData
    {
        internal string id = "null";
        internal List<KeyCode> keyCodes = new List<KeyCode>();
        internal Dictionary<string, object> data = new Dictionary<string, object>();

        internal ElementData(string id, KeyCode keyCode, Dictionary<string, object> data)
        {
            this.id = id;
            this.keyCodes = new List<KeyCode> { keyCode };
            this.data = data;
        }
        internal ElementData(string id, List<KeyCode> keyCodes, Dictionary<string, object> data)
        {
            this.id = id;
            this.keyCodes = keyCodes;
            this.data = data;
        }

    }
}
