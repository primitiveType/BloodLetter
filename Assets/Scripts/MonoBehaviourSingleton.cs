using UnityEngine;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : Object 
{
    private static T s_instance;

    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = Object.FindObjectOfType<T>();
            }

            return s_instance;
        }
        private set { s_instance = value; }
    }
}