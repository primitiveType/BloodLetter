using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private int index;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadSceneAsync(index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
