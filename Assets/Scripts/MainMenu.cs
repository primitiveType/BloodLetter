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

    // Update is called once per frame
    private void Update()
    {
    }

    public void NewGameClicked()
    {
        //ScreenWipeManager.Instance.DoWipe(m_camera);
        LevelManager.Instance.StartNewGame();
    }

    
}