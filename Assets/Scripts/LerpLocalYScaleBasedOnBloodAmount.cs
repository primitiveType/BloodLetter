using UnityEngine;

public class LerpLocalYScaleBasedOnBloodAmount : MonoBehaviour
{
    [SerializeField] private float m_MinScale;
    [SerializeField] private float m_MaxScale;

    // Start is called before the first frame update
    void Start()
    {
        Toolbox.Instance.PlayerRoot.ActorEvents.OnAmmoChangedEvent += ActorEventsOnOnAmmoChangedEvent;
        UpdateScale();
    }

    private void ActorEventsOnOnAmmoChangedEvent(object sender, OnAmmoChangedEventArgs args)
    {
        if (args.Type == AmmoType.Blood)
        {
            UpdateScale();
        }
    }

    private void UpdateScale()
    {
        float lerpValue = Toolbox.Instance.PlayerInventory.GetAmmoAmount(AmmoType.Blood) /
                          Toolbox.Instance.PlayerInventory.GetMaxAmmoAmount(AmmoType.Blood);
        var scale = transform.localScale;

        scale.y = Mathf.Lerp(m_MinScale, m_MaxScale, lerpValue);
        transform.localScale = scale;
    }

    private void OnDestroy()
    {
        Toolbox.Instance.PlayerRoot.ActorEvents.OnAmmoChangedEvent -= ActorEventsOnOnAmmoChangedEvent;
    }
}