using UnityEngine;

public class ExplodeOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject ExplosionPrefab;

    [SerializeField] private Vector3 LocalOffset = Vector3.up;
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnTrigger()
    {
        var explosion = Instantiate(ExplosionPrefab, transform);
        explosion.transform.localPosition = LocalOffset;
    }
}