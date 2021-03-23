using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnPlayer : MonoBehaviour
{
    private void Awake()
    {
        //force it to be synchronous.
        var result = Addressables.LoadAssetAsync<GameObject>("Player").Result;
        result.transform.SetParent(this.transform, false);
        result.transform.position = new Vector3();
        result.transform.rotation = Quaternion.identity;
    }
}