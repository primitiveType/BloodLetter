using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BloodLetterEditor
{
    [CustomEditor(typeof(EnemyDataProvider))]
    public class EnemyDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var t = target as EnemyDataProvider;

            if (GUILayout.Button("Save To Disk"))
            {
                SaveToDisk(t);
            }

            if (GUILayout.Button("Load From Disk"))
            {
                LoadFromDisk(t);
            }

            if (GUILayout.Button("Load From Components"))
            {
                LoadFromComponents(t);
            }
        }

        private void LoadFromComponents(EnemyDataProvider enemyDataProvider)
        {
            var startData = enemyDataProvider.Data;
            EnemyAggroHandler aggro = enemyDataProvider.GetComponentInChildren<EnemyAggroHandler>();
            MonsterAttackComponent attack = enemyDataProvider.GetComponentInChildren<MonsterAttackComponent>();
            MonsterVisibilityHandler visibilityHandler =
                enemyDataProvider.GetComponentInChildren<MonsterVisibilityHandler>();
            ActorHealth health = enemyDataProvider.GetComponentInChildren<ActorHealth>();
            ActorArmor armor = enemyDataProvider.GetComponentInChildren<ActorArmor>();
            //EnemyMovement movement = enemyDataProvider.GetComponentInChildren<EnemyMovement>();
            OnShotGivePlayerBlood canBleed = enemyDataProvider.GetComponentInChildren<OnShotGivePlayerBlood>();
            INavigationAgent navigation = enemyDataProvider.GetComponentInChildren<INavigationAgent>();


            if (visibilityHandler)
            {
                startData.DegreesVisibility = visibilityHandler.DegreesVisibility;
                startData.MaxHealth = (int) health.MaxHealth;
                startData.StartHealth = (int) health.Health;
                startData.OverhealMaxHealth = health.OverhealMaxHealth;
            }

            if (attack)
            {
                startData.Attacks = attack.AttackNames.ToList();
            }

            if (armor)
            {
                startData.MaxArmor = (int) armor.MaxArmor;
                startData.StartArmor = (int) armor.CurrentArmor;
                startData.OverhealMaxArmor = armor.OverhealMaxArmor;
            }

            startData.CanBleed = canBleed;

            if (navigation != null)
            {
                startData.Acceleration = navigation.Acceleration;
                startData.MoveSpeed = navigation.MoveSpeed;
                startData.StoppingDistance = navigation.StoppingDistance;
            }

            if (navigation is FlyingNavigation)
            {
                startData.IsFlying = true;
            }

            startData.AggroRange = aggro.AggroRange;
            startData.AggroDelayVariance = aggro.AggroDelayVariance;
            startData.CanAggro = aggro.enabled;
            startData.EarshotAggroRange = aggro.EarshotAggroRange;


            enemyDataProvider.Data = startData;
        }

        private void LoadFromDisk(EnemyDataProvider enemyDataProvider)
        {
            Debug.Log("Loading from disk.");

            enemyDataProvider.Data = GameConstants.GetEnemyDataByName(enemyDataProvider.EnemyName, "Normal");
        }

        private void SaveToDisk(EnemyDataProvider dataProvider)
        {
            string path = GameConstants.GetEnemyDataPath(dataProvider.EnemyName);
            Debug.Log($"Saving to disk: {path}");

            var json = JsonUtility.ToJson(dataProvider.Data);
            File.WriteAllText(path
                ,
                json);
        }
    }
}