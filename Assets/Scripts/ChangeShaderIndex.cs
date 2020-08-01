using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChangeShaderIndex : MonoBehaviour
{
    public int Max = 8;

    [SerializeField] private GameObject materialGameObject;

    private Renderer m_MyRenderer;

    private Renderer MyRenderer =>
        m_MyRenderer != null ? m_MyRenderer : m_MyRenderer = materialGameObject.GetComponent<Renderer>();

    private Transform mainCam;
    private static readonly int Perspective = Shader.PropertyToID("Perspective");

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Toolbox.Instance.PlayerHeadTransform;
    }

    public void TestFunction(string testParam)
    {
    }

    private void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            FixedUpdate();
        }
    }


    void FixedUpdate()
    {
        // float index = MyRenderer.GetFloat(Perspective);
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        MyRenderer.GetPropertyBlock(block);
        block.SetFloat(Perspective, GetFacingIndex());
        MyRenderer.SetPropertyBlock(block);
        // MyMat.SetFloat(Perspective, GetFacingIndex());
        // transform.LookAt(mainCam.transform);
    }

    //DRAGONS HERE
    private float GetFacingIndex()
    {
        //perspective 0 is front facing.
        //moving clockwise, numbers go up to 7
        var camTransform = mainCam.transform;
        var camPosition3 = camTransform.position;
        var camPosition = new Vector3(camPosition3.x, 0, camPosition3.z);
        var position = transform.position;
        var myPosition = new Vector3(position.x, 0, position.z);
        Vector3 camDirection = Vector3.Normalize(myPosition - camPosition);
        Debug.DrawLine(position, position + camDirection);
        // Debug.Log("CamDirection" + camDirection);

        // Debug.Log(camDirection);
        var halfStep = 22.5F;
        float angle = Vector3.SignedAngle( camDirection, transform.forward, Vector3.up) + 180 + halfStep ;
        //if offsetting it caused it to be negative or more than 360, shift it to account.
        if (angle < 0)
        {
            angle = (360) + angle;
        }
        else
        {
            angle = angle % 360;
        }
        //we now have the 0-360 angle where 0 and 360 are facing the player
        var index = (angle / 360f) * Max;
        return index;
    }

}