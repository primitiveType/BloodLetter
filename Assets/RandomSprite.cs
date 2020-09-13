using System.Collections;
using System.Collections.Generic;
using CodingEssentials.Collections;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public SpriteRenderer renderer;
    public List<Sprite> Sprites;
    // Start is called before the first frame update
    void Awake()
    {
        renderer.sprite = Sprites.RandomItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
