using System.Linq;
using SensorToolkit;
using UnityEngine;


[RequireComponent(typeof(Sensor))]
public class ShootDirectionProvider : MonoBehaviour, IShootDirectionProvider
{
    public bool ClassicAim;//TODO: move to settings

    private Sensor m_Sensor;
    
    public Vector3 ShootDirection 
    {
        get
        {
            if (ClassicAim)
            {
                return GetClassicAimDirection();
            }

            return transform.forward;
        }
    }

    public Sensor Sensor
    {
        get => m_Sensor != null ? m_Sensor : (m_Sensor = GetComponent<Sensor>());
    }

    private Vector3 GetClassicAimDirection()
    {
        ActorRoot closest = Sensor.DetectedObjectsOrderedByDistance.Select(go => go.GetComponentInParent<ActorRoot>()).FirstOrDefault(root => root != null);
        if (closest != null)
        {
            Vector3 position = transform.position;
            return (closest.Collider.ClosestPoint(position) - position).normalized;
        }

        return transform.forward;
    }
}