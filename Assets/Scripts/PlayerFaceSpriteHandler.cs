using System;
using UnityEngine;
using UnityEngine.UI;

internal class PlayerFaceSpriteHandler : MonoBehaviour
{
    [SerializeField] private Image Hat;
    [SerializeField] private Image Glasses;
    [SerializeField] private Image Eyebrows;
    [SerializeField] private Image Mouth;
    [SerializeField] private Image Eyes;
    [SerializeField] private Image OverHair;
    [SerializeField] private Image Nose;
    [SerializeField] private Image HairFull;
    [SerializeField] private Image HairUnder;
    [SerializeField] private Image HeadShape;
    [SerializeField] private Image Shoulder;
    [SerializeField] private Image Neck;
    [SerializeField] private Image HeadTop;
    [SerializeField] private Animator FaceAnimator;

    private void Start()
    {
        Toolbox.Instance.PlayerEvents.PlayerInteractEvent += PlayerEventsOnPlayerInteractEvent;
        Toolbox.Instance.PlayerEvents.OnDeathEvent += PlayerEventsOnOnDeathEvent;
        Toolbox.Instance.PlayerEvents.OnHealthChangedEvent += PlayerEventsOnOnHealthChangedEvent;
    }

    private void OnDestroy()
    {
        Toolbox.Instance.PlayerEvents.PlayerInteractEvent -= PlayerEventsOnPlayerInteractEvent;
        Toolbox.Instance.PlayerEvents.OnDeathEvent -= PlayerEventsOnOnDeathEvent;
        Toolbox.Instance.PlayerEvents.OnHealthChangedEvent -= PlayerEventsOnOnHealthChangedEvent;
    }

    private void PlayerEventsOnOnHealthChangedEvent(object sender, OnHealthChangedEventArgs args)
    {
    }

    private void PlayerEventsOnOnDeathEvent(object sender, OnDeathEventArgs args)
    {
    }

    private void PlayerEventsOnPlayerInteractEvent(object sender, PlayerInteractEventArgs args)
    {
    }
}