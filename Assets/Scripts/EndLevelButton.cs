using UnityEngine;

public class EndLevelButton : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        LevelManager.Instance.EndLevel();
    }
}