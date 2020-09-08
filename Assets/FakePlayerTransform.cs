using UnityEngine;

public class FakePlayerTransform : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Toolbox.Instance.SetPlayerTransform(transform);
        Toolbox.Instance.SetPlayerHeadTransform(transform);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}