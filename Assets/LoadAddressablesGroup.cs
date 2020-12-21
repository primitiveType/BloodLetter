using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadAddressablesGroup : MonoBehaviour
{
    private void Start()
    {
        Addressables.LoadAssetsAsync<GameObject>("Enemies", o => { });
    }
}