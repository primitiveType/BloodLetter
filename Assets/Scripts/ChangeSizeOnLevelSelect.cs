using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LevelReferenceComponent))]
public class ChangeSizeOnLevelSelect : MonoBehaviour
{
    [SerializeField] private float m_UnselectedSize = 1f;
    [SerializeField] private float m_SelectedSize = 5f;
    [SerializeField] private float m_GrowthTime = 1f;
    [SerializeField] private bool ChangeDepth;

    private LevelInfo Level { get; set; }
    private Coroutine ScaleCoroutine { get; set; }

    private float t;

    private void Start()
    {
        Level = GetComponent<LevelReferenceComponent>().Level;
        LevelSelectManager.Instance.OnLevelSelected += InstanceOnOnLevelSelected;
    }

    private void InstanceOnOnLevelSelected(object sender, LevelSelectedEventArgs args)
    {
        if (ScaleCoroutine != null)
        {
            StopCoroutine(ScaleCoroutine);
        }

        ScaleCoroutine = StartCoroutine(args.Level == Level ? AnimateGrow() : AnimateShrink());
    }

    private IEnumerator AnimateGrow()
    {
        if (ChangeDepth)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y, -2);
            transform.localPosition = localPosition;
        }

        float Easing(float start, float end)
        {
            t += Time.deltaTime;
            return EasingFunction.EaseOutQuad(start, end, t);
        }

        yield return AnimateScale(m_UnselectedSize, m_SelectedSize, Easing);
        transform.localScale = new Vector3(m_SelectedSize, m_SelectedSize, m_SelectedSize);

    }

    private IEnumerator AnimateScale(float start, float end, Func<float, float, float> easing)
    {
        while (t <= m_GrowthTime && t >= 0)
        {
            yield return null;
            var scale = easing(start, end);
            transform.localScale = new Vector3(scale, scale, scale);
        }

        t = Mathf.Clamp(t, 0, m_GrowthTime);
    }

    private IEnumerator AnimateShrink()
    {
        float Easing(float start, float end)
        {
            t -= Time.deltaTime;
            return EasingFunction.EaseOutQuad(start, end, t);
        }

        yield return AnimateScale(m_UnselectedSize, m_SelectedSize, Easing);
        transform.localScale = new Vector3(m_UnselectedSize, m_UnselectedSize, m_UnselectedSize);

        if (ChangeDepth)
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y, -1);
            transform.localPosition = localPosition;
        }
    }
}