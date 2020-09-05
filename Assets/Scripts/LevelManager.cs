using System;
using System.Collections.Generic;
using System.Net.Mime;
using E7.Introloop;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    private List<string> Levels = new List<string>
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
        UnityEditor.EditorApplication.isPlaying = false;
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
            {
                //ready to go to next level
                SceneToLoad = Levels[indexSceneToLoad];
            }
            else
            {
                //all levels complete, go to menu
                SceneToLoad = "MenuScene";
            }
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
        SceneManager.LoadScene(SceneToLoad);
        Timer.Instance.StartTimer();
    }
}

public delegate void LevelBeginEvent(object sender, LevelBeginEventArgs args);

public class LevelBeginEventArgs
{
}