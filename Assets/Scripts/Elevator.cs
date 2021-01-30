using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Elevator : MonoBehaviour, IInteractable
{
    private AudioSource audiosource;

    [SerializeField] private float delay = 2f;

    [FormerlySerializedAs("elevator")] [FormerlySerializedAs("rigidbody")] [SerializeField]
    protected Transform m_Elevator;

    protected Rigidbody rb;

    [FormerlySerializedAs("EndTarget")] [FormerlySerializedAs("TopTarget")] [SerializeField]
    protected Transform m_EndTarget;

    protected Coroutine MoveCR;
    [SerializeField] protected float predelay = 1;
    [SerializeField] public bool returns = true;
    public float speed = 10;

    [FormerlySerializedAs("StartTarget")] [FormerlySerializedAs("BottomTarget")] [SerializeField]
    protected Transform m_StartTarget;

    protected Vector3 StartPosition { get; set; }
    protected Vector3 EndPosition { get; set; }

    public Transform ElevatorTransform
    {
        get => m_Elevator;
        set => m_Elevator = value;
    }

    public Transform EndTarget
    {
        get => m_EndTarget;
        set => m_EndTarget = value;
    }

    public Transform StartTarget
    {
        get => m_StartTarget;
        set => m_StartTarget = value;
    }

    protected virtual void Start()
    {
        StartPosition = StartTarget.position;
        EndPosition = EndTarget.position;
        audiosource = GetComponent<AudioSource>();
        rb = ElevatorTransform.GetComponent<Rigidbody>();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(ElevatorTransform.position, .5f);
        Gizmos.DrawLine(StartTarget.transform.position, EndTarget.transform.position);
    }

    protected virtual IEnumerator Move(Vector3 target)
    {
        yield return new WaitForFixedUpdate();

        var start = ElevatorTransform.transform.position;
        // Vector3 start = transform.position;
        var distance = Vector3.Distance(start, target);
        float t = 0;
        var direction = (target - start).normalized;
        var targetVelocity = direction * speed;
        var approxTotalTime = distance / speed;
        while (t < 1f)
        {
            yield return new WaitForFixedUpdate();
            var currentTarget = Vector3.Lerp(start, target, t);
            // rb.velocity = targetVelocity;
            rb.MovePosition(currentTarget);
            t += Time.fixedDeltaTime /approxTotalTime ;
        }

        //elevator.transform.position = targetPosition;
        rb.MovePosition(target);
        rb.velocity = Vector3.zero;
        
        // rigidbody.position = targetPosition;
    }

    public void Trigger()
    {
        if (MoveCR != null)
            StopCoroutine(MoveCR);

        MoveCR = StartCoroutine(TriggerCr());
    }

    private IEnumerator TriggerCr()
    {
        yield return new WaitForSeconds(predelay);
        EnableAudio(true);

        var target = EndPosition;
        var returnTarget = StartPosition;

        yield return Move(target);

        if (returns)
        {
            EnableAudio(false);

            yield return new WaitForSeconds(delay);
            EnableAudio(true);

            yield return Move(returnTarget);
        }

        EnableAudio(false);
    }

    protected void EnableAudio(bool value)
    {
        if (audiosource)
        {
            audiosource.enabled = value;
            if (value && !audiosource.isPlaying)
            {
                audiosource.Play();
            }
        }
    }

    public bool Interact()
    {
        Trigger();
        return true;
    }
}