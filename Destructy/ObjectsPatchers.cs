using HarmonyLib;

namespace Destructy.Patch
{
    [HarmonyPatch(typeof(InventoryWorldItem))]
    internal static class WeaponPatcher
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void AwakePostfix(InventoryWorldItem __instance)
        {
            Lists.items.Add(__instance);
        }
    }
}
