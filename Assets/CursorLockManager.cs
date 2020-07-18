using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLockManager : MonoBehaviourSingleton<CursorLockManager>
{
    public void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
