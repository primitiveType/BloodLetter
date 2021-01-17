using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RotationSnapping : MonoBehaviour
{
    [SerializeField] private Transform mainCam;

    [SerializeField] private Transform sourceTransform;


    private void Update()
    {
        // float index = MyRenderer.GetFloat(Perspective);
        GetFacingValue();
        // MyMat.SetFloat(Perspective, GetFacingIndex());
        // transform.LookAt(mainCam.transform);
    }

    //DRAGONS HERE
    private float GetFacingValue()
    {
        if (mainCam == null) return 0;
        //perspective 0 is front facing.
        //moving clockwise, numbers go up to 7
        var camPosition3 = mainCam.position;
        var camPosition = new Vector3(camPosition3.x, 0, camPosition3.z);
        var position = sourceTransform.position;
        var myPosition = new Vector3(position.x, 0, position.z);
        var camDirection = Vector3.Normalize(myPosition - camPosition);
        Debug.DrawLine(position, position + camDirection);
        // Debug.Log("CamDirection" + camDirection);

        // Debug.Log(camDirection);
        var halfStep = 22.5F;
        var angle = Vector3.SignedAngle(camDirection, sourceTransform.forward, Vector3.up) + 180 + halfStep;
        //if offsetting it caused it to be negative or more than 360, shift it to account.
        if (angle < 0)
            angle = 360 + angle;
        else
            angle = angle % 360;
        //we now have the 0-360 angle where 0 and 360 are facing the player
        var index = angle / 360f * 8;
        int indexInt = Mathf.CeilToInt(index);
        transform.rotation = Quaternion.Euler(0, -angle + (indexInt * 45), 0);

        return index;
    }
}