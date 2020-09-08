using UnityEngine;

public class SaveState : MonoBehaviourSingleton<SaveState>
{
    [SerializeField] private SaveData _newGameSaveData;
    [SerializeField] private SaveData _saveData;

    public SaveData SaveData => _saveData;

    public void StartNewGame()
    {
        _saveData = new SaveData(_newGameSaveData);
    }
}