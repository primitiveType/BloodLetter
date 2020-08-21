
using UnityEngine;

public class PauseMenuUi : MonoBehaviour
{
    [SerializeField] private GameObject menuParent;
    private bool isPaused;

    public void Pause()
    {
        menuParent.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unpause()
    {
        menuParent.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void Quit()
    {
        LevelManager.Instance.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }
}