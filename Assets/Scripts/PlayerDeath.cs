using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private PlayerEvents Events;

    [SerializeField] private MonoBehaviour ToDisable;

    // Start is called before the first frame update
    private void Start()
    {
        Events.OnDeathEvent += EventsOnOnDeathEvent;
    }

    private void EventsOnOnDeathEvent(object sender, OnDeathEventArgs args)
    {
        ToDisable.enabled = false;
        StartCoroutine(LerpHead());
    }

    public IEnumerator LerpHead()
    {
        var headTransform = Toolbox.Instance.PlayerHeadTransform;
        var start = headTransform.localPosition;
        var end = start + Vector3.down;
        var time = .5f;
        float t = 0;
        while (t <= time)
        {
            t += Time.deltaTime;
            headTransform.localPosition = Vector3.Lerp(start, end, t / time);
            yield return null;
        }

            headTransform.localPosition = end;
    }

    private void OnDestroy()
    {
        Events.OnDeathEvent -= EventsOnOnDeathEvent;
    }
}