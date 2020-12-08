using UnityEngine;

public class ExplodeOnCollide : MonoBehaviour
{
    [SerializeField] private GameObject SpawnOnCollide;
    [SerializeField] private GameObject IgnoreCollision;

    public void SetIgnoreCollision(GameObject toIgnore)
    {
        IgnoreCollision = toIgnore;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == IgnoreCollision)
        {
            return;
        }
        Explode();
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            Debug.Log("Hit enemy!");
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            Debug.Log("Hit Player!");
        else
        {
            Debug.Log($"Hit environment! {other.gameObject.name}");
            foreach (var contact in other.contacts)
            {
                Debug.DrawRay(contact.point, Vector3.up, Color.yellow, 100);
            }
        }
    }


    private void Explode()
    {
        Destroy(gameObject);
        var spawned = Instantiate(SpawnOnCollide);
        spawned.transform.position = transform.position;
    }
}