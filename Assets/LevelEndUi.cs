using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEndUi : MonoBehaviour
{
    public Text YouDiedText;

    public Text TimeText;

    public Text EnemiesText;

    public Text SecretsText;

    public Button ContinueButton;

    // Start is called before the first frame update
    void Start()
    {
        TimeSpan test;
        test = Timer.Instance.GetTime();
        TimeText.text = $"{Mathf.FloorToInt((float)test.TotalMinutes).ToString()}:{test.Seconds:00}";

        CursorLockManager.Instance.Unlock();

        Toolbox.Instance.GetEnemyStatus(out int totalEnemies, out int deadEnemies);
        Toolbox.Instance.GetSecretStatus(out int totalSecrets, out int foundSecrets);

        YouDiedText.gameObject.SetActive(Toolbox.Instance.IsPlayerDead);
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