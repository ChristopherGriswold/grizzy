using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private GameObject cam;

	void LateUpdate ()
	{
        if (!cam)
        {
            if (Camera.main)
            {
                cam = Camera.main.gameObject;
            }
            return;
        }
        transform.rotation = cam.transform.rotation;
      //  transform.LookAt(cam.transform.position);
     //   transform.Rotate(0, 180, 0);
	}
}
