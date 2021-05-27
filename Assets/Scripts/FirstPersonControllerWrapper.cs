using System;
using System.Collections.Generic;
using ECM.Controllers;
using FPSBuilder.Core.Player;
using UnityEngine;

public class FirstPersonControllerWrapper : MonoBehaviour, IMovementHandler
{
    [SerializeField] private FirstPersonCharacterController m_Controller;

    // private BaseFirstPersonController Controller => m_Controller;
    private FirstPersonCharacterController Controller => m_Controller;

    private float BaseSpeed => m_BaseSpeed;

    [SerializeField] private float m_BaseSpeed = 40;

    List<MovementModifierHandle> Modifiers = new List<MovementModifierHandle>();

    public bool IsGrounded => Controller.Grounded;
    
    private void Awake()
    {
        SetRelativeSpeed(1);
    }

    public void SetRelativeSpeed(float percent)
    {
        var speed = BaseSpeed * percent;
        // Controller.
        // Controller.backwardSpeed = speed;
        // Controller.forwardSpeed = speed;
        // Controller.strafeSpeed = speed;
        // Controller.speed = speed;
    }

    public void AddMovementModifier(MovementModifierHandle handle)
    {
        Modifiers.Add(handle);
        handle.OnHandleChanged += OnHandleChanged;
        SetRelativeSpeed();
    }

    private void OnHandleChanged(object sender, HandleChangedArgs args)
    {
        SetRelativeSpeed();
    }

    private void SetRelativeSpeed()
    {
        float total = 0;
        foreach (var handle in Modifiers)
        {
            total += handle.ModifierPercentage;
        }

        SetRelativeSpeed(1 + total);
    }
}