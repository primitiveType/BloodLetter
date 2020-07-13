using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainHeight : MonoBehaviour
{
    private float worldHeight;
    // Start is called before the first frame update
    void Start()
    {
        worldHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        var myTransform = transform;
        var position = myTransform.position;
        position = new Vector3(position.x, worldHeight, position.z);
        myTransform.position = position;
    }
}
