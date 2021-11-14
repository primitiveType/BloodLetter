using UnityEngine;

public class InteractableIfSaveFileExists : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<UnityEngine.UI.Selectable>().interactable = SaveState.Instance.HasExistingSave();
    }
}