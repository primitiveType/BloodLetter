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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_rigidbody.velocity.normalized.y > .707)
        {
            spriteRenderer.sprite = Up.Random();
        }
        else if (_rigidbody.velocity.normalized.y < .1) //arbitrary number that feels good
        {
            spriteRenderer.sprite = Down.Random();
        }
        else
        {
            spriteRenderer.sprite = Sideways.Random();
        }
    }
}