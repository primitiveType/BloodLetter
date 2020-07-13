using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnTrigger : MonoBehaviour
{

    [SerializeField] private GameObject ExplosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTrigger()
    {
        var explosion = Instantiate(ExplosionPrefab, this.transform);
        explosion.transform.localPosition = Vector3.zero;
        
    }
}
