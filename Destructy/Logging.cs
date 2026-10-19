using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destructy.Loader;
namespace Destructy
{
    internal static class Logging
    {
        internal static void Log(string message) => LogInfo(message);
        internal static void LogInfo(string message)
        {
            Main.logger.LogInfo($" {message}");
        }
        internal static void LogWarning(string message)
        {
            Main.logger.LogWarning($" {message}");
        }
        internal static void LogError(string message)
        {
            Main.logger.LogError($" {message}");
        }
    }
}
