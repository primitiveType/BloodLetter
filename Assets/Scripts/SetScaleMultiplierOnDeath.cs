using UnityEngine;

public class SetScaleMultiplierOnDeath : MonoBehaviour
{
    [SerializeField] private ActorRoot root;

    [SerializeField] private Vector3 ScaleOnDeath;
    [SerializeField] private AnimationMaterialHelper m_MaterialHelper;


    // Start is called before the first frame update
    void Start()
    {
        root.ActorEvents.OnDeathEvent += OnDeathEvent;
    }

    private void OnDeathEvent(object sender, OnDeathEventArgs args)
    {
        m_MaterialHelper.ScaleMultiplier = ScaleOnDeath;
    }

    private void OnDestroy()
    {
        root.ActorEvents.OnDeathEvent -= OnDeathEvent;
    }
}