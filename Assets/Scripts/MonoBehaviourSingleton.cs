using UnityEngine;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
    private static T s_instance;

    public static T Instance
    {
        get
        {
            if (s_instance == null) s_instance = FindObjectOfType<T>();

            return s_instance;
        }
        private set => s_instance = value;
    }

    protected virtual void Awake()
    {
        if (Instance != this) Destroy(gameObject);
    }
}