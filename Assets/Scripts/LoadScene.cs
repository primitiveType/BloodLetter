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
        Debug.Log("Start");
        yield return new WaitForSeconds(.01f);
        Debug.Log("Initialize");
        yield return Addressables.InitializeAsync();
        Debug.Log("LoadScene");
        Addressables.LoadSceneAsync(name);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}