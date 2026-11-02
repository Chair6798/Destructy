using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destructy
{
    internal static class ChatMessagePresetFormat
    {
        internal static string Format(string preset, string message)
        {
            switch (preset.ToLower())
            {
                case "default":
                    return message;
                case "uppercase":
                    return message.ToUpper();
                case "lowercase":
                    return message.ToLower();
                case "titlecase":
                    return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(message);
                case "reverse":
                    char[] charArray = message.ToCharArray();
                    Array.Reverse(charArray);
                    return new string(charArray);
                case "rgb":
                    // Handle RGB color formatting if needed
                    int i = 0;
                    List<string> pack = new List<string>();
                    foreach (char ch in message.ToCharArray())
                    {
                        switch(i)
                        {
                            case 0:
                                pack.Add("<color=red>" + ch + "</color>");
                                break;
                            case 1:
                                pack.Add("<color=green>" + ch + "</color>");
                                break;
                            case 2:
                                pack.Add("<color=blue>" + ch + "</color>");
                                break;
                        }
                        i++;
                        if(i > 2)
                        {
                            i = 0;
                        }
                    }
                    return string.Join("", pack);
                default:
                    return message;
            }
        }
    }
}
