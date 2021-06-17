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
        public const string Version = "1.0.0.0";

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
                    mob_V.multiplier = 1.8f;
                    mob_V.bossMultiplier = 1.5f;
                }
                #endregion
            }
        }
    }
}
