using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using HarmonyLib;

namespace EHM.Core.Patches
{
    [HarmonyPatch(typeof(DayCycle), "Update")]
    class CyclePatch
    {
        [HarmonyPostfix]
        static void SetNightDuration(DayCycle __instance)
        {
            __instance.nightDuration = 0.9f;
        }
    }
}
