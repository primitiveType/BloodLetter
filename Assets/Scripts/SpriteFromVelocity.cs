using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SpriteFromVelocity : FromVelocity
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> Up;
    [SerializeField] private List<Sprite> Down;
    [SerializeField] private List<Sprite> Sideways;


    protected override void VelocityChanged(Direction newDirection)
    {
        switch (newDirection)
        {
            case Direction.Up:
                if (Up.Any())
                {
                    spriteRenderer.sprite = Up.Random();
                }

                break;
            case Direction.Down:
                if (Down.Any())
                {
                    spriteRenderer.sprite = Down.Random();
                }

                break;
            case Direction.Sideways:
                if (Sideways.Any())
                {
                    spriteRenderer.sprite = Sideways.Random();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}