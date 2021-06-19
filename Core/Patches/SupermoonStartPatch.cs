using HarmonyLib;
namespace EHM.Core.Patches
{
    [HarmonyPatch(typeof(GameLoop), "Update")]
    class SupermoonStartPatch
    {
        [HarmonyPostfix]
        static void StartSupermoon(int ___currentDay)
        {
            if (___currentDay % 7 == 0 && ___currentDay != 0)
                Main.superMoon = true;
        }
    }
}
