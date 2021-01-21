using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraY : MonoBehaviour
{
    private GameObject cam;
    Quaternion rot;
    void LateUpdate()
    {
        if (!cam)
        {
            if (Camera.main)
            {
                cam = Camera.main.gameObject;
            }
            return;
        }
        rot = Quaternion.LookRotation(transform.position - cam.transform.position, Vector3.up);
        transform.rotation = rot;
        //  transform.LookAt(cam.transform.position);
        //   transform.Rotate(0, 180, 0);
    }
}
