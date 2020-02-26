using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3.0F;
    public float strafeSpeed = 3.0f;

    void Update()
    {
        transform.Translate(Vector3.forward * CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime * moveSpeed);
        transform.Translate(Vector3.right * CrossPlatformInputManager.GetAxis("Horizontal") * Time.deltaTime * strafeSpeed);
    }

}
