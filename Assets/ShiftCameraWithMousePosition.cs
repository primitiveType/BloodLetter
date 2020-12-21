using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftCameraWithMousePosition : MonoBehaviour
{
    public float amount;

    private Vector3 m_startPosition;
    // Start is called before the first frame update
    void Start()
    {
        m_startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement =  (GetNormalizeMousePosition() *  amount);
        transform.position = m_startPosition + new Vector3(movement.x, movement.y, 0);
    }

    private Vector2 GetNormalizeMousePosition()
    {
        return Input.mousePosition / new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
    }
}