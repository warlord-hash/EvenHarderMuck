using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;

namespace EHM_RingsGaloreCompatabilityPatch
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInDependency("studzy.EHM.items",BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("YaBoiAlex_RingsGaloreApi", BepInDependency.DependencyFlags.HardDependency)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]

        public const string
            MODNAME = "EMH_RingsGaloreCompatabilityPatch",
            AUTHOR = "",
            GUID = AUTHOR + "_" + MODNAME,
            VERSION = "1.0.0.0";

        internal readonly ManualLogSource log;
        internal readonly Assembly assembly;
        public readonly string modFolder;

        #endregion

        public Main()
        {
            log = Logger;
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);
        }

        public void Start()
        {
            EHM.Items.Main.changedUI = true;
            RingsGaloreApi.Main.SingleSlot = true;
            RingsGaloreApi.Main.AddItemToRingSlot(ItemManager.Instance.GetItemByName("Steel Ring"));
            RingsGaloreApi.Main.AddItemToRingSlot(ItemManager.Instance.GetItemByName("Mithril Ring"));
            RingsGaloreApi.Main.AddItemEffects("Steel Ring", new RingsGaloreApi.AccessoryComponent { FlatSpeed = .15f });
            RingsGaloreApi.Main.AddItemEffects("Mithril Ring", new RingsGaloreApi.AccessoryComponent { FlatAttackSpeed = .10f });
        }
    }
}
