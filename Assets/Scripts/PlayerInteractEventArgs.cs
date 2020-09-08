public class PlayerInteractEventArgs
{
    public PlayerInteractEventArgs(IInteractable target)
    {
        Target = target;
    }

    public IInteractable Target { get; }
}