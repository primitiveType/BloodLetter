using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEndUi : MonoBehaviour
{
    public Text EnemiesText;

    public Text SecretsText;

    public Button ContinueButton;

    // Start is called before the first frame update
    void Start()
    {
        CursorLockManager.Instance.Unlock();
        Toolbox.Instance.GetEnemyStatus(out int totalEnemies, out int deadEnemies);
        Toolbox.Instance.GetSecretStatus(out int totalSecrets, out int foundSecrets);

        EnemiesText.text = $"Enemies Killed : {deadEnemies}/{totalEnemies}";
        SecretsText.text = $"Secrets Found : {foundSecrets}/{totalSecrets}";
        ContinueButton.onClick.AddListener(LoadNextLevel);
    }

    private void LoadNextLevel()
    {
        CursorLockManager.Instance.Lock();
        LevelManager.Instance.PreStartLevel();
        LevelManager.Instance.StartNextLevel();
    }
}