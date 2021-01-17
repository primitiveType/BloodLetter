using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFromVelocity : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> Up;
    [SerializeField] private List<Sprite> Down;
    [SerializeField] private List<Sprite> Sideways;
    [SerializeField] private Rigidbody _rigidbody;

    private enum Direction
    {
        Up,
        Down,
        Sideways
    }

    private Direction currentDirection;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Direction newDirection;
        if (_rigidbody.velocity.normalized.y > .707)
        {
            newDirection = Direction.Up;
        }
        else if (_rigidbody.velocity.normalized.y < .1) //arbitrary number that feels good
        {
            newDirection = Direction.Down;
        }
        else
        {
            newDirection = Direction.Sideways;
        }

        if (newDirection != currentDirection)
        {
            currentDirection = newDirection;
            switch (currentDirection)
            {
                case Direction.Up:
                    spriteRenderer.sprite = Up.Random();
                    break;
                case Direction.Down:
                    spriteRenderer.sprite = Down.Random();
                    break;
                case Direction.Sideways:
                    spriteRenderer.sprite = Sideways.Random();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}