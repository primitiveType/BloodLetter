
using UnityEngine;

public class PauseMenuUi : MonoBehaviour
{
    [SerializeField] private GameObject menuParent;
    private bool isPaused;

    public void Pause()
    {
        menuParent.SetActive(true);
        Time.timeScale = 0;
       CursorLockManager.Instance.Unlock();
    }

    public void Unpause()
    {
        menuParent.SetActive(false);
        Time.timeScale = 1;
        CursorLockManager.Instance.Lock();

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
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.B))
        {
            CursorLockManager.Instance.Unlock();
        }

        #endif
        
    }
}