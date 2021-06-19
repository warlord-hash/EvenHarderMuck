using HarmonyLib;
using UnityEngine;
using System.Reflection;

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
            var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;

            if (__instance.hunger == 0 && __instance.hp > 0)
                __instance.hp -= 0.01f;
            else if (__instance.hp <= 0)
                typeof(PlayerStatus).GetMethod("PlayerDied", flags).Invoke(__instance, new object[0]);
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
