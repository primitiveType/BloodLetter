public class EndLevelButton : BaseInteractable, IInteractable
{
    protected override void DoInteraction()
    {
        LevelManager.Instance.EndLevel();
    }
}