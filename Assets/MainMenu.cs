using System;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject NewGameMenu;
    [SerializeField] private GameObject TopLevel;

    // Start is called before the first frame update
    private void Start()
    {
        CursorLockManager.Instance.Unlock();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void NewGameClicked()
    {
        LevelManager.Instance.StartNewGame();
    }

    
}