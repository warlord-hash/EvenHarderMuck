using HarmonyLib;
using JetBrains.Annotations;

namespace EHM.Core.Patches
{
    class PowerupCalculationPatch
    {
    }
    
    [HarmonyPatch(typeof(PowerupInventory), "GetSpeedMultiplier")]
    class SpeedCalculationsPatch
    {
        [HarmonyPostfix]
        static void RemoveSpeedCap([CanBeNull] int[] playerPowerups, ref float __result)
        {
            float single = PowerupInventory.CumulativeDistribution(playerPowerups[ItemManager.Instance.stringToPowerupId["Sneaker"]], 0.0003f, 300f);
            float adrenalineBoost = 1f;
            if (PlayerStatus.Instance.adrenalineBoost)
            {
                adrenalineBoost = PowerupInventory.Instance.GetAdrenalineBoost(null);
            }

            __result = (1f + single) * adrenalineBoost * PlayerStatus.Instance.currentSpeedArmorMultiplier;
        }
    }

    [HarmonyPatch(typeof(PowerupInventory), "GetMaxDraculaStacks")]
    class RemoveDraculaMaxStack
    {
        [HarmonyPostfix]
        static void Remove(ref int __result)
        {
            __result = int.MaxValue;
        }
    }
}
