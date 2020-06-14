using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Camera ToFace;
    // Start is called before the first frame update
    void Awake()
    {
        ToFace = Camera.main;
        FaceThePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        FaceThePlayer();
    }

    private void FaceThePlayer()
    {
        var position = new Vector3(ToFace.transform.position.x, this.transform.position.y, ToFace.transform.position.z);
        transform.LookAt(position);
    }
}
