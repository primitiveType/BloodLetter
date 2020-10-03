using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string name;

    // Start is called before the first frame update
    private void Start()
    {
        Addressables.LoadSceneAsync(name);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}