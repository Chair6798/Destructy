using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Destructy
{
    internal class Element
    {
        internal string header;
        internal KeyCode bind;
        internal Dictionary<string, object> data;
        internal virtual string GetRenderText()
        {
            return header == string.Empty ? "NULL" : header;
        }
        internal Element(string header)
        {
            this.header = header;
        }
        internal virtual void Use()
        {
            Logging.Log("Base class of element data is used. Nothing happend.");
        }

    }
    internal class BoolElement : Element
    {
        internal bool value;
        internal UnityEvent onEnabled = new UnityEvent();
        internal UnityEvent onDisabled = new UnityEvent();
        internal UnityEvent<bool> onToggle = new UnityEvent<bool>();
        internal BoolElement(string header, bool value = false) : base(header)
        {
            this.value = value;
        }
        internal override string GetRenderText()
        {
            return base.GetRenderText() + ": " + value.ToString();
        }
        internal override void Use()
        {
            value = !value;
            if (value)
            {
                onEnabled.Invoke();
            }else
            {
                onDisabled.Invoke();
            }
            onToggle.Invoke(value);
            Logging.Log($"BoolElement used. Header: {GetRenderText()} New value: {value}");
        }
    }
    internal class FuncElement : Element
    {
        internal UnityEvent Event = new UnityEvent();
        internal FuncElement(string header) : base(header){}
        internal override void Use()
        {
            Event.Invoke();
        }
    }
}
