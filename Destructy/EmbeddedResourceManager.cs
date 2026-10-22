using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Destructy
{
    internal static class EmbeddedResourceManager
    {
        internal static void Check()
        {
            foreach (var res in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                Console.WriteLine(res);
            }
        }
        internal static StreamReader Get(string resId)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resId);
            if (stream == null)
            {
                Logging.LogError("Failed to get embedded resource: " + resId);
                return null;
            }
            return new StreamReader(stream);
        }
    }
}
