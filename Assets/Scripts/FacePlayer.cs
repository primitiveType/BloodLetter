using UnityEngine;

[ExecuteInEditMode]
public class FacePlayer : MonoBehaviour
{
    [SerializeField] private Transform ToFace;

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
        if (Application.isEditor && !Application.isPlaying)
            if (Camera.current)
                ToFace = Camera.current.transform;

        FaceThePlayer();
    }

    private void FaceThePlayer()
    {
        if (ToFace == null) return;
        var playerPosition = ToFace.position;
        var position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
        transform.LookAt(position);
    }
}