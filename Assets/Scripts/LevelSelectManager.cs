public class LevelSelectManager : MonoBehaviourSingleton<LevelSelectManager>
{
    public event LevelSelectedEvent OnLevelSelected;
    public LevelInfo CurrentSelectedLevel { get; private set; }

    public void SelectLevel(LevelInfo level)
    {
        CurrentSelectedLevel = level;
        OnLevelSelected?.Invoke(this, new LevelSelectedEventArgs(level));
    }
}