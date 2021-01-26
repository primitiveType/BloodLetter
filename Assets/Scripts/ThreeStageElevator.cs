using System;
using System.Collections;
using CodingEssentials;
using RealtimeCSG.Components;
using UnityEngine;
using UnityEngine.Serialization;

public class ThreeStageElevator : Elevator
{
    [SerializeField] private InteractableKey Key;

    [FormerlySerializedAs("MiddleTarget")] [SerializeField]
    private Transform m_MiddleTarget;

    public Transform MiddleTarget
    {
        get => m_MiddleTarget;
        set => m_MiddleTarget = value;
    }

    protected Vector3 MiddlePosition { get; set; }

    protected override void Start()
    {
        base.Start();
        MiddlePosition = MiddleTarget.position;
        ThreeStageElevatorIdService.Instance.RegisterInteractable(this, Key);
    }


    private void MoveToPosition(Vector3 target)
    {
        if (MoveCR != null)
        {
            EnableAudio(false);
            StopCoroutine(MoveCR);
        }

        MoveCR = StartCoroutine(TriggerCr(target));
    }


    public void MoveTo(ElevatorState state)
    {
        switch (state)
        {
            case ElevatorState.Start:
                MoveToStart();
                break;
            case ElevatorState.Middle:
                MoveToMiddle();
                break;
            case ElevatorState.End:
                MoveToEnd();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    public void MoveToStart()
    {
        MoveToPosition(StartPosition);
    }

    public void MoveToMiddle()
    {
        MoveToPosition(MiddlePosition);
    }

    public void MoveToEnd()
    {
        MoveToPosition(EndPosition);
    }

    private IEnumerator TriggerCr(Vector3 target)
    {
        yield return new WaitForSeconds(predelay);
        EnableAudio(true);


        yield return Move(target);

        EnableAudio(false);
    }

    protected override void OnDrawGizmos()
    {
        var color = Gizmos.color;
        Gizmos.color = Color.blue;
        if (ElevatorTransform)
        {
            Gizmos.DrawWireSphere(ElevatorTransform.position, .5f);
        }

        if (StartTarget && MiddleTarget)
        {
            Gizmos.DrawLine(StartTarget.transform.position, m_MiddleTarget.transform.position);
        }

        Gizmos.color = Color.gray;
        if (MiddleTarget && EndTarget)
        {
            Gizmos.DrawLine(m_MiddleTarget.transform.position, EndTarget.transform.position);
        }

        Gizmos.color = color;
    }

#if UNITY_EDITOR

    public void GenerateMissingTargets()
    {
        StartTarget = new GameObject("Start").transform;
        StartTarget.transform.SetParent(ElevatorTransform);
        StartTarget.localPosition = new Vector3();

        MiddleTarget = new GameObject("Middle").transform;
        MiddleTarget.transform.SetParent(ElevatorTransform);
        MiddleTarget.localPosition = new Vector3(0, 2.5f, 0);

        EndTarget = new GameObject("End").transform;
        EndTarget.transform.SetParent(ElevatorTransform);
        EndTarget.localPosition = new Vector3(0, 5, 0);
    }

    public void GenerateTrigger()
    {
        var triggerGo = new GameObject("Trigger");
        triggerGo.transform.SetParent(ElevatorTransform, false);
        var box = triggerGo.GetComponent<BoxCollider>();
        if (!box)
        {
            box = triggerGo.AddComponent<BoxCollider>();
        }

        box.isTrigger = true;
        box.GetOrAddComponent<ElevatorTrigger>().Elevator = this;

        var collider = ElevatorTransform.GetComponent<Collider>();
        if (collider)
        {
            box.size = collider.bounds.size;
        }
        else
        {
            var csgModel = ElevatorTransform.GetComponent<CSGModel>();
            Bounds? bounds = null;
            foreach (var mesh in csgModel.generatedMeshes.MeshInstances)
            {
                if (bounds == null)
                {
                    bounds = mesh.CachedMeshRenderer.bounds;
                }
                else
                {
                    if (mesh?.CachedMeshCollider != null)
                    {
                        bounds.Value.Encapsulate(mesh.CachedMeshCollider.bounds);
                    }
                }
            }

            if (bounds == null)
            {
                bounds = new Bounds(Vector3.zero, Vector3.one * 5);
            }

            box.size = bounds.Value.size;
        }

        box.center = new Vector3(0, box.size.y, 0);
    }

    public void GenerateSwitch()
    {
        throw new NotImplementedException();
    }
#endif
}