public class LevelEndEventArgs
{
    public LevelEndEventArgs(bool success)
    {
        Success = success;
    }

    public bool Success { get; set; }
}