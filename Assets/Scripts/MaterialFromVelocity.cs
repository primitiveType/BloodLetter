using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialFromVelocity : FromVelocity
{
    [SerializeField] private Renderer m_Renderer;
    [SerializeField] private List<Material> Up;
    [SerializeField] private List<Material> Down;
    [SerializeField] private List<Material> Sideways;

    protected override void VelocityChanged(Direction newDirection)
    {
        switch (newDirection)
        {
            case Direction.Up:
                if (Up.Any())
                {
                    m_Renderer.sharedMaterial = Up.Random();
                }

                break;
            case Direction.Down:
                if (Down.Any())
                {
                    m_Renderer.sharedMaterial = Down.Random();
                }

                break;
            case Direction.Sideways:
                if (Sideways.Any())
                {
                    m_Renderer.sharedMaterial = Sideways.Random();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}