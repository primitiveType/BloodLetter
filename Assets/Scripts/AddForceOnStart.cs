using UnityEngine;

public class AddForceOnStart : MonoBehaviour
{
    [SerializeField] private float ForceToAdd;

    [SerializeField] private float lobBias;

    // Start is called before the first frame update
    private void Start()
    {
        var target = GetComponent<Target>();
        if (target)
        {
            Vector3 start = transform.position;
            var rotation = AimRotation(start, target.Value.transform.position, ForceToAdd);
            GetComponent<Rigidbody>().velocity = rotation * (Vector3.forward * ForceToAdd);
        }
        else
        {
            var dir = transform.forward;
            var lobAmount = transform.up * lobBias;
            dir = (dir + lobAmount).normalized;
            GetComponent<Rigidbody>().AddForce(ForceToAdd * dir);
        }
    }


    private Quaternion AimRotation(Vector3 start, Vector3 end, float velocity)
    {
        float low;
        //float high;
        Ballistics.CalculateTrajectory(start, end, velocity, out low); //, out high); //get the angle


        Vector3 wantedRotationVector = Quaternion.LookRotation(end - start).eulerAngles; //get the direction
        wantedRotationVector.x = low; //combine the two
        return Quaternion.Euler(wantedRotationVector); //into a quaternion
    }
}