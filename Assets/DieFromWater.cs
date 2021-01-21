using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieFromWater : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13)
        {

            gameObject.GetComponent<AudioSource>().Play();
            other.gameObject.transform.root.GetComponentInChildren<HealthController>().Die();
        }
    }
}
