using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndUi : MonoBehaviour
{
    public Button ContinueButton;

    public Text EnemiesText;

    public Text SecretsText;

    public Text TimeText;
    public Text YouDiedText;

    // Start is called before the first frame update
    private void Start()
    {
        TimeSpan test;
        test = Timer.Instance.GetTime();
        TimeText.text = $"{Mathf.FloorToInt((float) test.TotalMinutes).ToString()}:{test.Seconds:00}";

        CursorLockManager.Instance.Unlock();

        Toolbox.Instance.GetEnemyStatus(out var totalEnemies, out var deadEnemies);
        Toolbox.Instance.GetSecretStatus(out var totalSecrets, out var foundSecrets);

        YouDiedText.gameObject.SetActive(Toolbox.Instance.IsPlayerDead);
        EnemiesText.text = $"Enemies Killed : {deadEnemies}/{totalEnemies}";
        SecretsText.text = $"Secrets Found : {foundSecrets}/{totalSecrets}";
        ContinueButton.onClick.AddListener(LoadNextLevel);
    }

    private void LoadNextLevel()
    {
        CursorLockManager.Instance.Lock();
        LevelManager.Instance.LoadLevelSelect();
    }
}