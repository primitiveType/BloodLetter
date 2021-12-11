using System;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;

public abstract class ProjectileInfoBase : MonoBehaviour
{
    //TODO: remove these?
    [SerializeField] private float m_MinRange = 10;
    [SerializeField] private float m_Range = 100;
    [SerializeField] private DamageType type;
    [SerializeField] private float m_Force;

    public float Force
    {
        get => m_Force;
        set => m_Force = value;
    }

    public DamageType Type => type;

    public float MinRange => m_MinRange;

    public float Range => m_Range;

    //public abstract void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection, EntityType ownerType);
    public abstract bool TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot, GameObject target);

    public void PopulateValues(string projectileName)
    {
        //get json for projectileName
        //populate damage, etc
        var data = GameConstants.GetProjectileDataByName(projectileName);
        PopulateData(data);
    }

    protected virtual void PopulateData(ProjectileData data)
    {
        m_MinRange = data.MinRange;
        m_Force = data.Force;
        m_Range = data.Range;
        type = data.type;
    }


    public virtual ProjectileData GetData()
    {
        var data = new ProjectileData();
        data.Name = name;
        data.Force = Force;
        data.type = Type;
        data.Range = Range;
        data.MinRange = MinRange;
        data.Prefab = name;
        return data;
    }
}

[Serializable]
public struct ProjectileData
{
    [SerializeField] public string Name;
    [SerializeField] public float MinRange;
    [SerializeField] public float Range;
    [SerializeField] public DamageType type;
    [SerializeField] public float Force;
    [SerializeField] public string Prefab;
    [SerializeField] public float Damage;
    [SerializeField] public string SubProjectileName;
    [SerializeField] public float SweepMagnitude;
    [SerializeField] public int NumToSpawn;
    [SerializeField] public float Duration;
}