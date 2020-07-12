using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    public void EndLevel()
    {
        SceneManager.LoadScene("LevelEnd");
    }

    public void PreStartLevel()
    {
        Toolbox.Instance.CleanupForNextLevel();
    }

    public void StartNextLevel()
    {
        SceneManager.LoadScene("ProBuilderScene");
    }
}