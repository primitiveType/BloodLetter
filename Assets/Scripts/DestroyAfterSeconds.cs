using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:pooling
public class DestroyAfterSeconds : MonoBehaviour
{
    public float TimeAlive = 1f;

    private float m_spawnTime;
    // Start is called before the first frame update
    void Start()
    {
        m_spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - m_spawnTime > TimeAlive)
        {
            Destroy(gameObject);
        }
    }
}
