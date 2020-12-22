public class LevelSelectManager : MonoBehaviourSingleton<LevelSelectManager>
{
    public event LevelSelectedEvent OnLevelSelected;
    public string CurrentSelectedLevel { get; private set; }

    public void SelectLevel(string levelName)
    {
        CurrentSelectedLevel = levelName;
        OnLevelSelected?.Invoke(this, new LevelSelectedEventArgs(levelName));
    }
}