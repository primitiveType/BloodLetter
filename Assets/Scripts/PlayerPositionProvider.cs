using UnityEngine;

public class PlayerPositionProvider : MonoBehaviour, IPositionProvider
{
    private CharacterController Controller { get; set; }
    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        Toolbox.Instance.SetPlayerBodyPosition(this);
    }

    public Vector3 GetPosition()
    {
        return transform.TransformPoint(Controller.center);
    }
}