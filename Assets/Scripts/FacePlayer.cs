using UnityEngine;

[ExecuteInEditMode]
public class FacePlayer : MonoBehaviour
{
    [SerializeField] private Transform ToFace;
    [SerializeField] private bool lockYAxis = true;

    private Transform ObjectToFace
    {
        get
        {
            if (Application.isEditor && !Application.isPlaying)
                if (Camera.current)
                    return Camera.current.transform;

            return ToFace;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (ToFace == null)
        {
            ToFace = Toolbox.Instance.PlayerHeadTransform;
        }

        FaceThePlayer();
    }

    // Update is called once per frame
    private void Update()
    {
      

        FaceThePlayer();
    }

    private void FaceThePlayer()
    {
        if (ObjectToFace == null) return;
        var playerPosition = ObjectToFace.position;
        var position = new Vector3(playerPosition.x, lockYAxis ? transform.position.y : playerPosition.y, playerPosition.z);
        transform.LookAt(position);
    }
}