using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class EquipStatus : MonoBehaviour
{
    private static readonly int Equipped = Animator.StringToHash("Equipped");

    [SerializeField] private Animator Animator;
    [SerializeField] private WeaponEventsHandler Events;
    private Transform m_transformToLerp;

    [FormerlySerializedAs("WeaponId")] [SerializeField]
    private WeaponId m_WeaponId;

    private readonly float TimeToLerp = .2f;

    public WeaponId WeaponId => m_WeaponId;
    private Vector3 EquippedPosition => new Vector3(0, 0, 0);
    private Vector3 UnequippedPosition => new Vector3(0, -1, 0);
    public bool IsEquipped { get; private set; }

    private Transform TransformToLerp => m_transformToLerp != null ? m_transformToLerp : m_transformToLerp = transform;

    public void Start()
    {
        Toolbox.Instance.PlayerEvents.OnWeaponsChangedEvent += PlayerEventsOnOnWeaponsChangedEvent;
    }

    public bool CanEquip()
    {
        return Toolbox.Instance.PlayerInventory.HasWeapon(m_WeaponId);
    }

    public IEnumerator Equip()
    {
        IsEquipped = true;
        yield return StartCoroutine(LerpPosition(TransformToLerp.localPosition, EquippedPosition));
        Animator.SetBool(Equipped, IsEquipped);
    }

    public IEnumerator UnEquip()
    {
        IsEquipped = false;
        Animator.SetBool(Equipped, IsEquipped);
        yield return StartCoroutine(LerpPosition(TransformToLerp.localPosition, UnequippedPosition));
    }

    public void UnEquipInstant()
    {
        TransformToLerp.localPosition = UnequippedPosition;
    }

    private IEnumerator LerpPosition(Vector3 startPosition, Vector3 endPosition)
    {
        var speed = 1f / TimeToLerp;
        float t = 0;
        while (t < 1)
        {
            TransformToLerp.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            t += Time.deltaTime * speed;
            yield return null;
        }

        TransformToLerp.localPosition = endPosition;
    }

    private void PlayerEventsOnOnWeaponsChangedEvent(object sender, OnWeaponsChangedEventArgs args)
    {
        if ((args.NewValue ^ args.OldValue) == WeaponId) Toolbox.Instance.PlayerInventory.EquipThing(this);
    }

    private void OnDestroy()
    {
        Toolbox.Instance.PlayerEvents.OnWeaponsChangedEvent -= PlayerEventsOnOnWeaponsChangedEvent;
    }
}