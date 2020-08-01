using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FacePlayer : MonoBehaviour
{
    private Transform ToFace;
    // Start is called before the first frame update
    void Start()
    {
        ToFace = Toolbox.Instance.PlayerHeadTransform;
        FaceThePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        FaceThePlayer();
    }

    private void FaceThePlayer()
    {
        var playerPosition = ToFace.position;
        var position = new Vector3(playerPosition.x, this.transform.position.y, playerPosition.z);
        transform.LookAt(position);
    }
}
