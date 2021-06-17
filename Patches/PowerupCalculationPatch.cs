using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace EHM.Core.Patches
{
    class PowerupCalculationPatch
    {
    }
    
    [HarmonyPatch(typeof(PowerupInventory), "GetSpeedMultiplier")]
    class CalculationsPatch
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
}
