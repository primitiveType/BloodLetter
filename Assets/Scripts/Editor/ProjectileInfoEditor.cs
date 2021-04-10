using System.IO;
using UnityEditor;
using UnityEngine;

namespace BloodLetterEditor
{
    [CustomEditor(typeof(ProjectileInfoBase), true)]
    public class ProjectileInfoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var t = target as ProjectileInfoBase;


            if (GUILayout.Button("Load From Components"))
            {
                LoadFromComponents(t);
            }
        }

        private void LoadFromComponents(ProjectileInfoBase projectile)
        {
            ProjectileData data = projectile.GetData();
            SaveToDisk(data);
        }

        private void SaveToDisk(ProjectileData dataProvider)
        {
            string path = GameConstants.GetProjectileDataPath(dataProvider.Name);
            Debug.Log($"Saving to disk: {path}");

            var json = JsonUtility.ToJson(dataProvider);
            File.WriteAllText(path
                ,
                json);
        }
    }
}