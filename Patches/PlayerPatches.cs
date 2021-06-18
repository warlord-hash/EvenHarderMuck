using HarmonyLib;

namespace EHM.Core.Patches
{
    class PlayerPatches
    {
    }

    [HarmonyPatch(typeof(PlayerStatus), "Update")]
    class PlayerHungerPatch
    {
        [HarmonyPostfix]
        static void DecreaseHunger(PlayerStatus __instance)
        {
            if (__instance.hunger == 0)
                __instance.hp -= 0.05f;
        }
    }

    [HarmonyPatch(typeof(PlayerMovement), "Awake")]
    class PlayerSpeedPatch
    {
        [HarmonyPostfix]
        static void DecreaseSpeed(ref float ___maxSpeed, ref float ___maxWalkSpeed)
        {
            ___maxSpeed = 5.9f;
            ___maxWalkSpeed = 5.9f;
        }
    }
}
