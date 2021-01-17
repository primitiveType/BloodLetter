using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform m_RotateAround;

    public float m_Speed;

    public Vector3 m_Axis;

    private Light Light { get; set; }

    private Vector3 PreviousParentScale { get; set; } = Vector3.zero;

    [SerializeField] private float m_DistanceModifier = 1f;
    [SerializeField] private float m_IntensityModifier = .1f;

    private void Awake()
    {
        Light = GetComponent<Light>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_RotateAround.lossyScale != PreviousParentScale)
        {
            Vector3 lossyScale = m_RotateAround.lossyScale;
            PreviousParentScale = lossyScale;
            Light.range = lossyScale.magnitude;
            Light.intensity = lossyScale.sqrMagnitude * m_IntensityModifier;
            Vector3 parentPosition = m_RotateAround.position;
            Transform myTransform = transform;
            Vector3 difference = myTransform.position - parentPosition;
            Vector3 direction = difference.normalized;
            myTransform.position = parentPosition + direction * (m_DistanceModifier * lossyScale.magnitude);
        }

        transform.RotateAround(m_RotateAround.position, m_Axis, m_Speed);
    }
}
