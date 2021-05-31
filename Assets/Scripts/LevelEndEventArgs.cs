public class LevelEndEventArgs
{
    public LevelEndEventArgs(bool success, string levelName)
    {
        Success = success;
        LevelName = levelName;
    }

    public bool Success { get; set; }
    public string LevelName { get; set; }
}