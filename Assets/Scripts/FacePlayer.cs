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
        if (Application.isEditor && !Application.isPlaying)
        {
            if (Camera.current)
                ToFace = Camera.current.transform;
        }

        FaceThePlayer();
    }

    private void FaceThePlayer()
    {
        if (ToFace == null)
        {
            return;
        }
        var playerPosition = ToFace.position;
        var position = new Vector3(playerPosition.x, this.transform.position.y, playerPosition.z);
        transform.LookAt(position);
    }
}