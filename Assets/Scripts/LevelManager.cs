using System.Collections.Generic;
using E7.Introloop;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    private readonly List<string> Levels = new List<string>
    {
        "TheCageScene",
        "Overgrowth"
    };

    private string SceneToLoad = "TheCageScene";

    public event LevelEndEvent LevelEnd;
    public event LevelBeginEvent LevelBegin;

    protected override void Awake()
    {
        base.Awake();
        //probably a bad idea!
        // cachedInventory = Toolbox.Instance.PlayerInventory.GetPersistentData();
    }

    public void StartFromLevel(int index)
    {
        SceneToLoad = Levels[index];
        SaveState.Instance.StartNewGame();
        PreStartLevel();
        StartNextLevel();
    }

    public void Quit()
    {
        //handle saving to disk here, probably
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        Debug.Log(Application.companyName);//this is just here to keep a reference to application...
#else
        Application.Quit();
#endif
    }

    public void EndLevel(bool success)
    {
        if (success)
        {
            var indexSceneToLoad = Levels.IndexOf(SceneToLoad) + 1;
            if (indexSceneToLoad > 0 && Levels.Count > indexSceneToLoad)
                //ready to go to next level
                SceneToLoad = Levels[indexSceneToLoad];
            else
                //all levels complete, go to menu
                SceneToLoad = "MenuScene";
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

    public void StartNextLevel()
    {
        Addressables.LoadSceneAsync(SceneToLoad);
        //SceneManager.LoadScene(SceneToLoad);
        Timer.Instance.StartTimer();
    }
}