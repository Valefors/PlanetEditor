using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameObject _hooverObject = null;

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                EventsManager.Instance.Raise(new OnMouseClick(hit.transform.gameObject));
                _hooverObject = null;
                return;
            }

            //We don't want to send events at each frame
            if (_hooverObject == hit.transform.gameObject) return;

            //If the detected object and the previous hoover object aren't the same, it means
            //that the mouse has moved. We can unhoover the previous object
            if (_hooverObject != null) EventsManager.Instance.Raise(new OnMouseExit(_hooverObject));

            _hooverObject = hit.transform.gameObject;
            EventsManager.Instance.Raise(new OnMouseHoover(_hooverObject));
        }

        //We need to detect if the mouse doesn't hit anything for unselected effect
        else
        {
            if (_hooverObject != null) EventsManager.Instance.Raise(new OnMouseExit(_hooverObject));
            _hooverObject = null;

            if (Input.GetMouseButtonDown(0))
            {
                EventsManager.Instance.Raise(new OnMouseClick());
            }
        }
    }
}
