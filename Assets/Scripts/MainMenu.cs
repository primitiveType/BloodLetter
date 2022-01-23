using System;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject NewGameMenu;
    [SerializeField] private GameObject TopLevel;
    [SerializeField] private Camera m_camera;

    // Start is called before the first frame update
    private void Start()
    {
        CursorLockManager.Instance.Unlock();
    }


    public void NewGameClicked()
    {
        //ScreenWipeManager.Instance.DoWipe(m_camera);
        NewGameMenu.SetActive(true);
        TopLevel.SetActive(false);
    }

    public void StartEasyGame()
    {
        LevelManager.Instance.StartNewGame("I Found A Gun!");
    }

    public void StartMediumGame()
    {
        LevelManager.Instance.StartNewGame("Yeah, I'm Tough");
    }

    public void StartHardGame()
    {
        LevelManager.Instance.StartNewGame("Born To Kill");
    }

    public void ContinueClicked()
    {
        LevelManager.Instance.LoadGame();
    }
}