using UnityEngine;

public class AddForceOnStart : MonoBehaviour
{
    [SerializeField] private float ForceToAdd;

    [SerializeField] private float lobBias;

    // Start is called before the first frame update
    private void Start()
    {
        var dir = transform.forward;
        var lobAmount = transform.up * lobBias;
        dir = (dir + lobAmount).normalized;
        GetComponent<Rigidbody>().AddForce(ForceToAdd * dir);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}