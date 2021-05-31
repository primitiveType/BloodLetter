using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float bobbingAmount = 0.05f;
    public CharacterController controller;

    private float defaultPosY;
    private float timer;

    private Transform Transform;

    public float walkingBobbingSpeed = 14f;

    // Start is called before the first frame update
    private void Start()
    {
        Transform = transform;

        defaultPosY = Transform.localPosition.y;
       Toolbox.Instance.PlayerEvents.OnDeathEvent += OnPlayerDeath;
    }

    private void OnPlayerDeath(object sender, OnDeathEventArgs args)
    {
        Transform.localPosition = new Vector3(); 
        enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Mathf.Abs(controller.velocity.x) > 0.1f || Mathf.Abs(controller.velocity.z) > 0.1f)
        {
            //Player is moving
            timer += Time.deltaTime * walkingBobbingSpeed;
            Transform.localPosition = new Vector3(Transform.localPosition.x,
                defaultPosY + Mathf.Sin(timer) * bobbingAmount, Transform.localPosition.z);
        }
        else
        {
            //Idle
            timer = 0;
            Transform.localPosition = new Vector3(Transform.localPosition.x,
                Mathf.Lerp(Transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed),
                Transform.localPosition.z);
        }
    }
}