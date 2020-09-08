using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private AudioSource InteractAudioSource;
    [SerializeField] private float interactDistance = 1f;

    [SerializeField] private PlayerEvents PlayerEvents;

    // Start is called before the first frame update
    private void Awake()
    {
        Toolbox.Instance.SetPlayerHeadTransform(transform);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var foundOne = false;
            var transform1 = transform;
            var ray = new Ray(transform1.position, transform1.forward * interactDistance);
            Debug.DrawRay(transform1.position, transform1.forward * interactDistance, Color.blue, 10);

            if (Physics.Raycast(ray, out var hit, interactDistance, LayerMask.GetMask("Interactable")))
            {
                var interactables = hit.collider.GetComponentsInChildren<IInteractable>();

                foundOne = true;
                foreach (var interactable in interactables)
                {
                    foundOne &= interactable.Interact();
                    PlayerEvents.PlayerInteract(interactable);
                }
            }

            if (!foundOne) InteractAudioSource.Play();
        }
    }
}