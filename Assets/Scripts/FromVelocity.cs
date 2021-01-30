using UnityEngine;

public abstract class FromVelocity : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    protected enum Direction
    {
        Up,
        Down,
        Sideways
    }

    protected Direction currentDirection;

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
            VelocityChanged(newDirection);
        }
    }

    protected abstract void VelocityChanged(Direction newDirection);
}