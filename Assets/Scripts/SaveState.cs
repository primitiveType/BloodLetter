using System.Data.SqlTypes;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SaveState : MonoBehaviourSingleton<SaveState>
{
    [SerializeField] private SaveData _newGameSaveData;
    [SerializeField] private SaveData _saveData;

    public SaveData SaveData => _saveData;

    public void StartNewGame()
    {
        _saveData = new SaveData(_newGameSaveData);
        Save();
    }

    public bool HasExistingSave()
    {
        return File.Exists(SavePath);
    }

    private static string SavePath => Path.Combine(Application.persistentDataPath, "savedata.json");

    public void LoadSave()
    {
        _saveData = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(SavePath));
    }

    public void Save()
    {
        string json = JsonConvert.SerializeObject(_saveData);
        File.WriteAllText(SavePath, json);
    }
}