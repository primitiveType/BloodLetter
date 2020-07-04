using UnityEngine;

public class MonsterVisibilityHandler : MonoBehaviour
{   
    private Transform Target { get; set; }

    private Transform MonsterTransform { get; set; }
    private Transform TargetCollider { get; set; }

    private bool m_CanSeePlayer;

    private void Start()
    {
        Target = Toolbox.PlayerHeadTransform;
        TargetCollider = Toolbox.PlayerTransform;
        MonsterTransform = transform;
    }

    private int lastFrameCheck;
    private int checkFrequency = 2;
    
    public bool CanSeePlayer()
    {
        if (Time.frameCount < lastFrameCheck + checkFrequency) return m_CanSeePlayer;
       
        lastFrameCheck = Time.frameCount;

        var angle = Vector3.Dot(Target.position - MonsterTransform.position, MonsterTransform.forward);
        if (angle < 0) //if monster isn't facing player
        {
            m_CanSeePlayer =  false;
        }

        //might want to offset monster position so they can see over low walls, etc.
        Ray ray = new Ray(MonsterTransform.position, Target.position - MonsterTransform.position);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, LayerMask.GetMask("Player", "Default")))
        {
           return  m_CanSeePlayer =  hitInfo.transform == TargetCollider;
        }


        m_CanSeePlayer = false;

        return m_CanSeePlayer;
    }
}