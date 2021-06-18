using HarmonyLib;

namespace EHM.Core.Patches
{
    [HarmonyPatch(typeof(DayCycle), "Update")]
    class CyclePatch
    {
        [HarmonyPostfix]
        static void SetNightDuration(DayCycle __instance)
        {
            __instance.nightDuration = 0.7f;
        }
    }

    [HarmonyPatch(typeof(GameLoop), "NewDay")]
    class MobNightModifier
    {
        [HarmonyPostfix]
        static void SetMobNight(int day)
        {
            Main.mobNightMultiplier = (float)day / 10f;

            if (Main.mobNightMultiplier <= 0.0f)
                Main.mobNightMultiplier = 0.1f;
        }
    }
}
