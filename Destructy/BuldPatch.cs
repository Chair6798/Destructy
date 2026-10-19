using Destructy.Loader;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Destructy.Patch
{
    [HarmonyPatch(typeof(SettingsManager), "Awake")]
    internal static class BuildPatch
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            Logging.Log("Build patch called.");
            if (Main.obj != null)
            {
                Logging.LogWarning("Already builded!");
                return;
            }
            Main.obj = new GameObject("Destructy");
            Main.obj.AddComponent<Attaches.MainBehaviour>();
            Object.DontDestroyOnLoad(Main.obj);
        }
    }
}
