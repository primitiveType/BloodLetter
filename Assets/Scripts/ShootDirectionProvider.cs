using System.Collections.Generic;
using System.Linq;
using SensorToolkit;
using UnityEngine;


[RequireComponent(typeof(Sensor))]
public class ShootDirectionProvider : MonoBehaviour, IShootDirectionProvider
{
    public bool ClassicAim; //TODO: move to settings

    private Sensor m_Sensor;

    private List<Vector3> ShootDirections = new List<Vector3>();

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

    public List<Vector3> AllShootDirections
    {
        get
        {
            ShootDirections.Clear();
            foreach (var obj in Sensor.DetectedObjectsOrderedByDistance.Select(go => go.GetComponentInParent<ActorRoot>()))
            {
                Vector3 position = transform.position;
                ShootDirections.Add((obj.Collider.bounds.center - position).normalized);
            }

            return ShootDirections;
        }
    }

    public Sensor Sensor
    {
        get => m_Sensor != null ? m_Sensor : (m_Sensor = GetComponent<Sensor>());
    }

    private Vector3 GetClassicAimDirection()
    {
        ActorRoot closest = Sensor.DetectedObjectsOrderedByDistance.Select(go => go.GetComponentInParent<ActorRoot>())
            .FirstOrDefault(root => root != null);
        if (closest != null)
        {
            Vector3 position = transform.position;
            return (closest.Collider.ClosestPoint(position) - position).normalized;
        }

        return transform.forward;
    }
}