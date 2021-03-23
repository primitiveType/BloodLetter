using UnityEngine;

//TODO:pooling
public class DestroyAfterSeconds : MonoBehaviour
{
    private float m_spawnTime;

    public float TimeAlive = 1f;

    // Start is called before the first frame update
    private void Awake()
    {
        m_spawnTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.time - m_spawnTime > TimeAlive)
        {
            Destroy(gameObject);
        }
    }
}