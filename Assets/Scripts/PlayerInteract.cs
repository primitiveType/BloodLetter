using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactDistance = 1f;

    [SerializeField] private PlayerEvents PlayerEvents;

    [SerializeField] private AudioSource InteractAudioSource;

    // Start is called before the first frame update
    void Awake()
    {
        Toolbox.Instance.SetPlayerHeadTransform(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool foundOne = false;
            var transform1 = transform;
            Ray ray = new Ray(transform1.position, transform1.forward * interactDistance);
            Debug.DrawRay(transform1.position, transform1.forward * interactDistance, Color.blue, 10);

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, LayerMask.GetMask($"Interactable")))
            {
                var interactables = hit.collider.GetComponentsInChildren<IInteractable>();
                foundOne = interactables.Any();

                foreach (var interactable in interactables)
                {
                    interactable?.Interact();
                    PlayerEvents.PlayerInteract(interactable);
                }

            }

            if (!foundOne)
            {
                InteractAudioSource.Play();
            }
        }
    }
}


public interface IInteractable
{
    void Interact();
}