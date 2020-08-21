using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject TopLevel;

    [SerializeField] private GameObject NewGameMenu;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NewGameClicked()
    {
        TopLevel.SetActive(false);
        NewGameMenu.SetActive(true);
    }

    public void StartLevel(int index)
    {
        LevelManager.Instance.StartFromLevel(index);
    }
}