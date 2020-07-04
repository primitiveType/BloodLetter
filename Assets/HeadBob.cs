using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    public CharacterController controller;

    float defaultPosY = 0;
    float timer = 0;

    private Transform Transform;
    // Start is called before the first frame update
    void Start()
    {
        Transform = transform;

        defaultPosY = Transform.localPosition.y; 

    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(controller.velocity.x) > 0.1f || Mathf.Abs(controller.velocity.z) > 0.1f)
        {
            //Player is moving
            timer += Time.deltaTime * walkingBobbingSpeed;
            Transform.localPosition = new Vector3(Transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, Transform.localPosition.z);
        }
        else
        {
            //Idle
            timer = 0;
            Transform.localPosition = new Vector3(Transform.localPosition.x, Mathf.Lerp(Transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), Transform.localPosition.z);
        }
    }
}