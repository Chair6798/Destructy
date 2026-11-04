using BepInEx;
using BepInEx.Configuration;
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
        internal static ConfigEntry<KeyCode> menuToggle;
        void Awake()
        {
            Logger.LogInfo("Destructy plugin loading started!");
            harmony = new Harmony("chaircheats.Destructy");
            harmony.PatchAll();
            instance = this;
            logger = Logger;
            menuToggle = Config.Bind("General", "MenuToggle", KeyCode.F1, "Key to toggle the Destructy menu.");
            //EmbeddedResourceManager.Check();
            //GameFoldersManager.CheckAndInit();
            Logger.LogInfo("Destructy plugin loaded!");
            
        }
    }
}
