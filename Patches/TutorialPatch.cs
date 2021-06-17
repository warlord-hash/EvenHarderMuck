using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using UnityEngine;

namespace EHM.Core.Patches
{
    [HarmonyPatch(typeof(Tutorial), "Update")]
    class TutorialPatch
    {
        [HarmonyPrefix]
        static void EndTutorial(ref int ___progress)
        {
            ___progress = 1000;
        }
    }
}
