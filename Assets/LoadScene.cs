using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private int index;

    // Start is called before the first frame update
    private void Start()
    {
        SceneManager.LoadSceneAsync(index);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}