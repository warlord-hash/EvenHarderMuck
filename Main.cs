using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;

namespace EHM.Core
{
    [BepInPlugin(GUID, Name, Version)]
    public class Main : BaseUnityPlugin
    {
        public const string GUID = "studzy.EHM.core";
        public const string Name = "EHM Core";
        public const string Version = "1.0.2";

        public static bool superMoon = false;
        public static float mobNightMultiplier = 0.1f;
        public static List<string> mobChanged = new List<string>();

        internal void Awake()
        {
            Debug.Log("Loaded " + Name + " v" + Version);
            var harmony = new Harmony("com.studzy.ehmcore");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        internal void Update()
        {
            if (SceneManager.GetActiveScene().name == "GameAfterLobby")
            {
                #region EnemyAdjustmentsAndBuffs
                foreach (var mob in MobManager.Instance.mobs)
                {
                    Mob mob_V = mob.Value;
                    HitableMob component = mob_V.GetComponent<HitableMob>();

                    if(!mobChanged.Contains(component.entityName))
                    {
                        var lootdrop = component.dropTable;

                        for (int i = 0; i < lootdrop.loot.Length; i++)
                        {
                            if (lootdrop.loot[i].item.name == "Coin")
                            {
                                lootdrop.loot[i].amountMax = Mathf.FloorToInt(lootdrop.loot[i].amountMax / 2f);
                                lootdrop.loot[i].amountMin = Mathf.FloorToInt(lootdrop.loot[i].amountMin / 2f);
                            }
                        }

                        mobChanged.Add(component.entityName);
                    }

                    if (!superMoon)
                    {
                        mob_V.multiplier = 1.15f + 0.15f * GetPlayersAlive() + mobNightMultiplier;
                        mob_V.bossMultiplier = 1.25f + 0.15f * GetPlayersAlive() + mobNightMultiplier;
                    } else
                    {
                        mob_V.multiplier = 1.45f + 0.15f * GetPlayersAlive() + mobNightMultiplier;
                        mob_V.bossMultiplier = 1.55f + 0.15f * GetPlayersAlive() + mobNightMultiplier;
                    }
                }
                #endregion

                #region Supermoon
                MeshRenderer moonMesh = GameObject.Find("Moon 1").GetComponent<MeshRenderer>();
                if (superMoon)
                {
                    Color superMoonColor = new Color(240, 50, 36);
                    moonMesh.material.color = superMoonColor;
                } else
                {
                    moonMesh.material.color = new Color(0.8538f, 1f, 0.9847f);
                }
                #endregion
            }
        }

        // custom for optimization
        int GetPlayersAlive()
        {
            int num = 0;
            foreach (PlayerManager value in GameManager.players.Values)
            {
                if (!value || value.dead)
                {
                    continue;
                }
                num++;
            }
            return num;
        }
    }
}
