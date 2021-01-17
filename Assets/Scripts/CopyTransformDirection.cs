using UnityEngine;

public class CopyTransformDirection : MonoBehaviour
{
    private Transform myTransform;

    [SerializeField] private Transform toCopy;

    // Start is called before the first frame update
    private void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        myTransform.rotation = toCopy.rotation;
    }
}