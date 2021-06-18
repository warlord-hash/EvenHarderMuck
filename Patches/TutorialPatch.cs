using HarmonyLib;

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
