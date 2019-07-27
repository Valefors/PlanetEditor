using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotX = 0f;
    public float rotY = 0f;

    public float mouseDragX = 500f;
    public float mouseDragY = 500f;

    void Update()
    {
        if (!Input.GetMouseButton(1)) return;

        rotX += Input.GetAxis("Mouse X") * mouseDragX * Time.deltaTime;
        rotY += Input.GetAxis("Mouse Y") * mouseDragY * Time.deltaTime;
        transform.localEulerAngles = new Vector3(rotY, -rotX, 0);
    }
}
