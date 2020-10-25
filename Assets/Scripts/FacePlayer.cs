using UnityEngine;

[ExecuteInEditMode]
public class FacePlayer : MonoBehaviour
{
    [SerializeField] private Transform ToFace;

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
        var position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
        transform.LookAt(position);
    }
}