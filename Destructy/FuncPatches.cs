using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace Destructy
{
    [HarmonyPatch(typeof(NetworkPlayer))]
    internal static class NetworkPlayerPatches
    {
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static bool Update_Prefix(NetworkPlayer __instance)
        {
            //if()
            return true;
        }
    }
}
