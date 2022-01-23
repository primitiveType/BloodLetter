using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerDataProvider))]
public class PlayerDataEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var t = target as PlayerDataProvider;

        if (GUILayout.Button("Save To Disk"))
        {
            SaveToDisk(t);
        }

        if (GUILayout.Button("Load From Disk"))
        {
            LoadFromDisk(t);
        }
    }


    private void LoadFromDisk(PlayerDataProvider dataProvider)
    {
        Debug.Log("Loading from disk.");

        dataProvider.Data = GameConstants.GetPlayerData("Normal");
    }

    private void SaveToDisk(PlayerDataProvider dataProvider)
    {
        string path = GameConstants.GetPlayerDataPath(dataProvider.Data.difficulty);
        Debug.Log($"Saving to disk: {path}");

        var json = JsonUtility.ToJson(dataProvider.Data);
        File.WriteAllText(path, json);
    }
}