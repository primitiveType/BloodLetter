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
            var rotation = AimRotation(transform.position, target.transform.position, ForceToAdd);
            GetComponent<Rigidbody>().AddForce(ForceToAdd * rotation.eulerAngles);
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