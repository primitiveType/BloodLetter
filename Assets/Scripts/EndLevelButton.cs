public class EndLevelButton : BaseInteractable, IInteractable
{
    protected override bool DoInteraction()
    {
        LevelManager.Instance.EndLevel();
        return true;
    }
}