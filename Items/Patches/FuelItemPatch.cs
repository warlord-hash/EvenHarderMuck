using HarmonyLib;
using UnityEngine;

namespace EHM.Items.Patches
{
    [HarmonyPatch(typeof(FurnaceSync), "CanProcess")]
    class FuelItemPatch
    {
        [HarmonyPostfix]
        static void AddNewProcessCheck(FurnaceSync __instance, ref bool __result)
        {
            if(__instance.cells[0] != null && __instance.cells[1] != null)
            {
                if(__instance.cells[0].id == 130)
                {
                    if (__instance.gameObject.transform.parent.gameObject.name.Contains("Forge") == false)
                        __result = false;
                }
            }
        }
    }
}
