using HarmonyLib;
using System.Reflection;

namespace EHM.Items.Patches
{
    class PowerupMultipliersPatch
    {
    }

    [HarmonyPatch(typeof(PowerupInventory), "GetSpeedMultiplier")]
    class SpeedMultiplierPatch
    {
        public static bool set = false;
        static void Postfix(ref float __result)
        {
            if(Main.accs != null)
            {
                if(Main.accs.GetComponent<InventoryCell>().currentItem != null && Main.accs.GetComponent<InventoryCell>().currentItem.name == "Steel Ring")
                {
                    __result += 0.15f;
                    set = true;
                }
            }
        }
    }


    [HarmonyPatch(typeof(PowerupInventory), "GetAttackSpeedMultiplier")]
    class AttackSpeedMultiplierPatch
    {
        public static bool set = false;
        static void Postfix(ref float __result)
        {
            if (Main.accs != null)
            {
                if (Main.accs.GetComponent<InventoryCell>().currentItem != null && Main.accs.GetComponent<InventoryCell>().currentItem.name == "Mithril Ring")
                {
                    __result += 0.10f;
                    set = true;
                }
            }
        }
    }
}
