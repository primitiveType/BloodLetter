public class LevelSelectedEventArgs
{
    public LevelInfo Level { get; }

    public LevelSelectedEventArgs(LevelInfo level)
    {
        Level = level;
    }
}