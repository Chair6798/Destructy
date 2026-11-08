using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Destructy
{
    internal class Element
    {
        internal string header;
        internal KeyCode bind;
        internal bool hostOnly = false;
        internal virtual string GetRenderText()
        {
            return header == string.Empty ? "NULL" : header;
        }
        internal Element(string header, bool hostOnly=false)
        {
            this.header = header;
            this.hostOnly = hostOnly;
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

        internal ref bool GetValueRef()
        {
            return ref value;
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
        internal bool Avaiable = true;
        internal override string GetRenderText()
        {
            return Avaiable ? base.GetRenderText() : ("<color=red><i>" + base.GetRenderText() + "</i></color>");
        }
        internal FuncElement(string header, bool hostOnly = false) : base(header, hostOnly){}
        internal override void Use()
        {
            if(Avaiable)
                Event.Invoke();
        }
    }
    internal class StringElement : Element
    {
        internal string value;
        internal StringElement(string header, string value = "", bool hostOnly = false) : base(header, hostOnly)
        {
            this.value = value;
        }
        internal ref string GetValueRef()
        {
            return ref value;
        }
    }
    internal class EnumElement : Element
    {
        internal int value;
        internal List<string> options;
        internal EnumElement(string header, List<string> options, int value = 0, bool hostOnly = false) : base(header, hostOnly)
        {
            this.options = options;
            this.value = value;
        }
        internal override string GetRenderText()
        {
            return base.GetRenderText() + ": " + options[value];
        }
        internal override void Use()
        {
            value++;
            if (value >= options.Count)
            {
                value = 0;
            }
            Logging.Log($"EnumElement used. Header: {GetRenderText()} New value: {value}");
        }
    }
    internal class FloatElement : Element
    {
        internal float value = 0f;
        internal float min = 0f;
        internal float max = 1f;
        internal float defaultValue = 0f;
        internal FloatElement(string header, float value = 0f, float min = 0f, float max = 1f, bool hostOnly=false) : base(header, hostOnly)
        {
            this.value = value;
            this.defaultValue = value;
            this.min = min;
            this.max = max;
        }
        internal void Reset()
        {
            value = defaultValue;
        }
        internal ref float GetValueRef()
        {
            return ref value;
        }
    }
}
