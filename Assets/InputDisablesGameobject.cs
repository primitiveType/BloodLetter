using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDisablesGameobject : MonoBehaviour
{
    [SerializeField] private KeyCode Key;
    [SerializeField] private GameObject m_ObjectToDisable;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            m_ObjectToDisable.SetActive(!m_ObjectToDisable.activeSelf);
        }
    }
}
