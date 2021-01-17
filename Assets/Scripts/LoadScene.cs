using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string name;

    private void Start()
    {
        StartCoroutine(StartCR());
    }

    // Start is called before the first frame update
    private IEnumerator StartCR()
    {
        Debug.Log("STart");
        yield return new WaitForSeconds(1);
        Debug.Log("Initialize");
        Addressables.InitializeAsync();
        yield return new WaitForSeconds(5);
        Debug.Log("LoadScene");
        Addressables.LoadSceneAsync(name);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}