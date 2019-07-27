using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSensibility = 1.0f;

    public float minZoom = 2.0f;
    public float maxZoom = 10.0f;

    float _zoom;

    private void Start()
    {
        _zoom = Mathf.Lerp(minZoom, maxZoom, 0.5f);
    }

    void Update()
    {

        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheelInput != 0.0f)
        {
            _zoom += zoomSensibility * mouseWheelInput;
            _zoom = Mathf.Clamp(_zoom, minZoom, maxZoom);
        }

        transform.position = transform.forward * -_zoom;
    }
}
