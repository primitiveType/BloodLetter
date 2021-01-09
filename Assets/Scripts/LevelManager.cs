using E7.Introloop;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    public event LevelEndEvent LevelEnd;
    public event LevelBeginEvent LevelBegin;


    public void StartLevel(string name)
    {
        // ScreenWipeManager.Instance.Capture(Camera.main);
        // ScreenWipeManager.Instance.DoWipe();
        // return;
        ScreenWipeManager.Instance.Capture(Camera.main, true);
        PreStartLevel();
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(name);
        handle.Completed += operationHandle =>
        {
            ScreenWipeManager.Instance.DoWipe();
            Timer.Instance.StartTimer();
        };
    }


    public void Quit()
    {
        //handle saving to disk here, probably
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        Debug.Log(Application.companyName); //this is just here to keep a reference to application...
#else
        Application.Quit();
#endif
    }

    public void EndLevel(bool success)
    { //TODO: handle end level screen and succes state...
        if (success)
        {
            LoadLevelSelect();
        }

        Timer.Instance.PauseTimer();
        LevelEnd?.Invoke(this, new LevelEndEventArgs(success));
        IntroloopPlayer.Instance.Stop();
        SceneManager.LoadScene("LevelEnd");
    }

    public void PreStartLevel()
    {
        LevelBegin?.Invoke(this, new LevelBeginEventArgs());
    }


    public void StartNewGame()
    {
        SaveState.Instance.StartNewGame();
        LoadLevelSelect();
    }

    public void LoadLevelSelect()
    {
        ScreenWipeManager.Instance.Capture(Camera.main, true);
        CursorLockManager.Instance.Unlock();
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync("Assets/LevelSelect.unity");
        handle.Completed += operationHandle => { ScreenWipeManager.Instance.DoWipe(); };
    }
}