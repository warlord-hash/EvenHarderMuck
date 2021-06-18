using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
using System.Reflection;

namespace EHM.Core
{
    [BepInPlugin(GUID, Name, Version)]
    public class Main : BaseUnityPlugin
    {
        public const string GUID = "studzy.EHM.core";
        public const string Name = "EHM Core";
        public const string Version = "1.0.1";

        public static bool superMoon = false;

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
                #region EnemyBuffs
                foreach (var mob in MobManager.Instance.mobs)
                {
                    Mob mob_V = mob.Value;
                    if (!superMoon)
                    {
                        mob_V.multiplier = 1.3f;
                        mob_V.bossMultiplier = 1.5f;
                    } else
                    {
                        mob_V.multiplier = 1.6f;
                        mob_V.bossMultiplier = 1.8f;
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
    }
}
