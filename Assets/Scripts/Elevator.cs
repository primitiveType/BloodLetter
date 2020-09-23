using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Elevator : MonoBehaviour
{
    private AudioSource audiosource;

    [SerializeField] private float delay = 2f;

    [FormerlySerializedAs("rigidbody")] [SerializeField]
    protected Transform elevator;

    protected Rigidbody rb;

    [FormerlySerializedAs("TopTarget")] [SerializeField]
    protected Transform EndTarget;

    protected Coroutine MoveCR;
    [SerializeField] protected float predelay;
    [SerializeField] protected bool returns = true;
    public float speed;

    [FormerlySerializedAs("BottomTarget")] [SerializeField]
    protected Transform StartTarget;

    protected virtual void Start()
    {
        audiosource = GetComponent<AudioSource>();
        rb = elevator.GetComponent<Rigidbody>();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(elevator.position, .5f);
        Gizmos.DrawLine(StartTarget.transform.position, EndTarget.transform.position);
    }

    protected virtual IEnumerator Move(Transform target)
    {
        yield return new WaitForFixedUpdate();

        var start = elevator.transform.position;
        // Vector3 start = transform.position;
        var targetPosition = target.position;
        var distance = Vector3.Distance(start, targetPosition);
        float t = 0;
        var direction = (targetPosition - start).normalized;
        var targetVelocity = direction * speed;
        var approxTotalTime = distance / speed;
        Debug.Log(approxTotalTime);
        while (t < 1f)
        {
            yield return new WaitForFixedUpdate();
            var currentTarget = Vector3.Lerp(start, targetPosition, t);
            // rb.velocity = targetVelocity;
            rb.MovePosition(currentTarget);
            t += Time.fixedDeltaTime /approxTotalTime ;
        }

        //elevator.transform.position = targetPosition;
        rb.MovePosition(targetPosition);
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

        var target = EndTarget;
        var returnTarget = StartTarget;

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
}