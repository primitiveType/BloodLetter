using UnityEngine;
using UnityEngine.UI;

public class ToastHandler : MonoBehaviourSingleton<ToastHandler>
{
    [SerializeField] private int max;
    [SerializeField] private Text toastPrefab;

    public void PopToast(string message)
    {
        if (transform.childCount >= max) Destroy(transform.GetChild(0).gameObject);

        var toast = Instantiate(toastPrefab, transform);
        toast.text = message;
    }
}