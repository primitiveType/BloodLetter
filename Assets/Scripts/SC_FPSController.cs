using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SC_FPSController : MonoBehaviour, IMovementHandler
{
    [HideInInspector] public bool canMove = true;

    private CharacterController characterController;
    public float gravity = 20.0f;
    public float jumpSpeed = 8.0f;
    public float lookSpeed => (4.0f * SettingsManager.Instance.Settings.Sensitivity) + .1f;
    public float lookXLimit = 45.0f;
    private Vector3 moveDirection = Vector3.zero;
    public Camera playerCamera;
    private float rotationX;
    public float runningSpeed = 11.5f;
    public float walkingSpeed = 7.5f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        MyCollider = GetComponent<CapsuleCollider>();
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        var forward = transform.TransformDirection(Vector3.forward);
        var right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        var isRunning = Input.GetKey(KeyCode.LeftShift);

        var vert = Input.GetAxis("Vertical");
        var horz = Input.GetAxis("Horizontal");

        var movedir = new Vector2(vert, horz);
        var targetSpeed = isRunning ? runningSpeed : walkingSpeed;
        movedir *= targetSpeed;
        if (movedir.magnitude > targetSpeed) movedir = movedir.normalized * targetSpeed;
        var curSpeedX = canMove ? movedir.x : 0;
        var curSpeedY = canMove ? movedir.y : 0;


        var movementDirectionY = moveDirection.y;
        moveDirection = forward * curSpeedX + right * curSpeedY;

        if (Input.GetButton("Jump") && canMove && (characterController.isGrounded || IsGrounded))
            moveDirection.y = jumpSpeed;
        else
            moveDirection.y = movementDirectionY;


        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded && !IsGrounded) moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove && Time.timeScale > 0)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(Bottom, OverlapRadius);
        }
    }

    private Collider[] collisions = new Collider[2];
    private CapsuleCollider MyCollider { get; set; }

    private Vector3 Bottom
    {
        get
        {
            var bounds = MyCollider.bounds;

            return new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
        }
    }

    private float OverlapRadius => MyCollider.radius / 2f;

    public void AddMovementModifier(MovementModifierHandle handle)
    {
        throw new NotImplementedException();
    }

    public bool IsGrounded
    {
        get
        {
            if (Physics.OverlapSphereNonAlloc(Bottom, OverlapRadius, collisions, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore) > 0)
                return true;

            return false;
        }
    }
}