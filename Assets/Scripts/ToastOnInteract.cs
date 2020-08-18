using System.Text;
using UnityEngine;

public class ToastOnInteract : MonoBehaviour, IInteractable
{
    public string toastMessage;
    public bool onlyOnce = true;
    private bool toasted;

    public bool Interact()
    {
        if (!toasted || !onlyOnce)
        {
            ToastHandler.Instance.PopToast(toastMessage);
            toasted = true;
            return true;
        }

        return false;
    }
}