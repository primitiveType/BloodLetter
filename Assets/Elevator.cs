using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float speed;
    [SerializeField] private Transform BottomTarget;

    [SerializeField] private Transform TopTarget;

    [SerializeField] private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move(TopTarget));
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator Move(Transform target)
    {

        Vector3 start = rigidbody.position;
        var targetPosition = target.position;
        float distance = Vector3.Distance(start, targetPosition);
        float t = 0;

        yield return null;
        while (t < 1f)
        {
            var currentTarget = Vector3.Lerp(start, targetPosition, t);
            rigidbody.MovePosition(currentTarget);
            yield return null;
            t += (Time.deltaTime) / (distance / speed);
        }

        rigidbody.position = targetPosition;
        yield return new WaitForSeconds(2f);
        if (target == TopTarget)
        {
            StartCoroutine(Move(BottomTarget));
        }
        else
        {
            StartCoroutine(Move(TopTarget));
        }
    } 
}