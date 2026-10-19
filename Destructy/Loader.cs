using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
namespace Destructy.Loader
{
    [BepInPlugin("chaircheats.Destructy", "Destructy", "1.0.0")]
    internal class Main : BaseUnityPlugin
    {
        internal static Main instance;
        internal static ManualLogSource logger;
        internal static Harmony harmony;
        internal static GameObject obj;
        void Awake()
        {
            Logger.LogInfo("Destructy plugin loading started!");
            harmony = new Harmony("chaircheats.Destructy");
            harmony.PatchAll();
            Logger.LogInfo("Destructy plugin loaded!");
            logger = Logger;
        }
    }
}
