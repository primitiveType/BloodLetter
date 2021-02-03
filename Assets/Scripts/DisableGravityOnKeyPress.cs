    using ECM.Components;
using UnityEngine;

public class DisableGravityOnKeyPress : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [SerializeField] private CharacterMovement m_movement;

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            m_movement.useGravity = !m_movement.useGravity;
        }
    }
}