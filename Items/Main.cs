using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EHM.Items
{
    [BepInPlugin(GUID, Name, Version)]
    public class Main : BaseUnityPlugin
    {
        public const string GUID = "studzy.EHM.items";
        public const string Name = "EHM Items";
        public const string Version = "1.0.0";

        public static InventoryItem[] items;
        public static bool inventoryAdded = false;

        internal void Awake()
        {
            Debug.Log("Loaded " + Name + " v" + Version);
            var items_a = GetAssetBundleFromResources("custom_items");
            items = items_a.LoadAllAssets<InventoryItem>();

            var harmony = new Harmony("com.studzy.ehmitems");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        internal void Start()
        {
            for (int i = 0; i < items.Length; i++)
            {
                Debug.Log(i);
                items[i].id = i + ItemManager.Instance.allScriptableItems.Length;
                ItemManager.Instance.allItems.Add(items[i].id, items[i]);
            }

            ItemManager.Instance.allScriptableItems = ItemManager.Instance.allScriptableItems.Concat(items).ToArray();

            InventoryItem adamantiteBar = ItemManager.Instance.GetItemByName("Adamantite bar");
            adamantiteBar.processable = true;
            adamantiteBar.processedItem = items[2];
            adamantiteBar.processTime = 15;

            for (int i = 0; i < items.Length; i++)
            {
                switch (items[i].id)
                {
                    case 129:
                        items[i].prefab.gameObject.layer = LayerMask.NameToLayer("Object");
                        items[i].prefab.gameObject.transform.Find("Interactable").gameObject.layer = LayerMask.NameToLayer("Interact");
                        items[i].prefab.gameObject.AddComponent<HitableChest>();
                        HitableChest hChest = items[i].prefab.GetComponent<HitableChest>();
                        hChest.entityName = "Forge";
                        hChest.hp = 100;
                        hChest.maxHp = 100;
                        hChest.compatibleItem = InventoryItem.ItemType.Pickaxe;
                        hChest.dropItem = ItemManager.Instance.GetItemByName("Forge");
                        hChest.amount = 1;
                        hChest.dontScale = true;

                        GameObject furnaceRef = ItemManager.Instance.GetItemByName("Furnace").prefab.gameObject;
                        hChest.destroyFx = furnaceRef.GetComponent<HitableChest>().destroyFx;
                        hChest.hitFx = furnaceRef.GetComponent<HitableChest>().hitFx;
                        hChest.numberFx = furnaceRef.GetComponent<HitableChest>().numberFx;
                        hChest.dropTable = furnaceRef.GetComponent<HitableChest>().dropTable;

                        Transform interactable_a = items[i].prefab.gameObject.transform.Find("Interactable");
                        GameObject interactable = interactable_a.gameObject;  

                        interactable.AddComponent<ChestInteract>();
                        interactable.AddComponent<FurnaceSync>();
                        ChestInteract cInt = interactable.GetComponent<ChestInteract>();
                        cInt.state = OtherInput.CraftingState.Furnace;

                        FurnaceSync fSync = interactable.GetComponent<FurnaceSync>();
                        fSync.processType = InventoryItem.ProcessType.Smelt;
                        fSync.inUse = false;
                        fSync.chestSize = 3;

                        List<InventoryItem.CraftRequirement> iList = new List<InventoryItem.CraftRequirement>(){
                            new InventoryItem.CraftRequirement(){item=ItemManager.Instance.GetItemByName("Iron bar"),amount=10},
                            new InventoryItem.CraftRequirement(){item=ItemManager.Instance.GetItemByName("Rock"),amount=25}
                        };

                        AddCraftable(items[i], ItemManager.Instance.GetItemByName("Workbench"), iList.ToArray());
                        break;
                    case 130:
                        List<InventoryItem.CraftRequirement> iList3 = new List<InventoryItem.CraftRequirement>(){
                            new InventoryItem.CraftRequirement(){item=ItemManager.Instance.GetItemByName("Flint"),amount=4},
                        };

                        AddCraftable(items[i], ItemManager.Instance.GetItemByName("Workbench"), iList3.ToArray());
                        break;
                    case 132:
                        List<InventoryItem.CraftRequirement> iList2 = new List<InventoryItem.CraftRequirement>(){
                            new InventoryItem.CraftRequirement(){item=ItemManager.Instance.GetItemByName("Trezolite Bar"),amount=7},
                            new InventoryItem.CraftRequirement(){item=ItemManager.Instance.GetItemByName("Oak Wood"),amount=5}
                        };

                        AddCraftable(items[i], ItemManager.Instance.GetItemByName("Anvil"), iList2.ToArray());
                        break;
                }
            }
        }

        internal void Update()
        {
            if (SceneManager.GetActiveScene().name == "GameAfterLobby")
            {
                if(!inventoryAdded)
                {
                    List<InventoryItem> workbenchList = new List<InventoryItem>{
                    ItemManager.Instance.GetItemByName("Forge"),
                };
                    Debug.Log(workbenchList.Count());
                    AddToStation(workbenchList, "WorkbenchNew", 2);

                    List<InventoryItem> workbenchList2 = new List<InventoryItem>{
                    ItemManager.Instance.GetItemByName("Glowpowder"),
                };
                    AddToStation(workbenchList2, "WorkbenchNew", 0);

                    List<InventoryItem> anvilList = new List<InventoryItem>{
                    ItemManager.Instance.GetItemByName("Trezolite Katana"),
                };
                    AddToStation(anvilList, "AnvilNew", 2);
                    inventoryAdded = true;
                }
            }
        }

        void AddCraftable(InventoryItem item, InventoryItem station, InventoryItem.CraftRequirement[] craftRequirements)
        {
            if (item.craftable)
            {
                item.requirements = item.requirements.Concat(craftRequirements).ToArray();
                item.stationRequirement = station;
            }
        }

        void AddToStation(List<InventoryItem> itemss, string stationName, int tabIndex)
        {
            var res = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == stationName);

            foreach (GameObject station in res)
            {
                CraftingUI ui = station.GetComponent<CraftingUI>();

                if (ui)
                {
                    InventoryItem[] iArray = itemss.ToArray();
                    ui.tabs[tabIndex].items = ui.tabs[tabIndex].items.Concat(iArray).ToArray();
                } else
                {
                    Debug.Log("BRUH PENIS");
                }
            }
        }

        public static AssetBundle GetAssetBundleFromResources(string fileName)
        {
            var execAssembly = Assembly.GetExecutingAssembly();

            var resourceName = execAssembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));

            using (var stream = execAssembly.GetManifestResourceStream(resourceName))
            {
                return AssetBundle.LoadFromStream(stream);
            }
        }
    }
}
