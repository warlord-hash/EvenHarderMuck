using HarmonyLib;

namespace EHM.Core.Patches
{
    [HarmonyPatch(typeof(DayCycle), "Update")]
    class CyclePatch
    {
        [HarmonyPostfix]
        static void SetNightDuration(DayCycle __instance)
        {
            __instance.nightDuration = 0.8f;
        }
    }
}
