using UnityEngine;

public class ExplodeOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject ExplosionPrefab;

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
        explosion.transform.localPosition = Vector3.zero;
    }
}