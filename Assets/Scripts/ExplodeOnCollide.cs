using System;
using JetBrains.Annotations;
using UnityEngine;

public class ExplodeOnCollide : MonoBehaviour
{
    [SerializeField] private GameObject SpawnOnCollide;
    [SerializeField] private bool m_InheritScale;
    
    [SerializeField] private GameObject IgnoreCollision;
    [SerializeField] private LayerMask Mask = ~0;

    public void SetIgnoreCollision(GameObject toIgnore)
    {
        IgnoreCollision = toIgnore;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnEnter(other.gameObject);
    }

    private void OnEnter(GameObject other)
    {
        if (!Mask.ContainsLayer(other.gameObject.layer))
        {
            return;
        }

        if (other.gameObject == IgnoreCollision)
        {
            return;
        }

        Explode();
    }

    private void OnCollisionEnter(Collision other)
    {
        OnEnter(other.gameObject);
        // if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        // {
        //     //Debug.Log("Hit enemy!");
        // }
        // else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        // {
        //     //Debug.Log("Hit Player!");
        // }
        // else
        // {
        //     //Debug.Log($"Hit environment! {other.gameObject.name}");
        //     foreach (var contact in other.contacts)
        //     {
        //         Debug.DrawRay(contact.point, Vector3.up, Color.yellow, 100);
        //     }
        // }
    }


    private void Explode()
    {
        Destroy(gameObject);
        if (SpawnOnCollide)
        {
            var spawned = Instantiate(SpawnOnCollide);
            spawned.transform.position = transform.position;

            if (m_InheritScale)
            {
                spawned.transform.localScale = transform.localScale;
            }
        }
    }
}