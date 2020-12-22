public class LevelSelectedEventArgs
{
    public string LevelName { get; }

    public LevelSelectedEventArgs(string levelName)
    {
        LevelName = levelName;
    }
}