using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateChat : MonoBehaviour
{
    private GameObject cam;

    void LateUpdate ()
	{
        if(!cam)
        {
            if (Camera.main)
            {
                cam = Camera.main.gameObject;
            }
            return;
        }
        transform.LookAt(cam.transform.position);
        transform.Rotate(0, 180, 0);
    }
}
