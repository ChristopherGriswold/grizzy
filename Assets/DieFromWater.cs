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
            other.gameObject.transform.root.GetComponentInChildren<HealthController>().Die();
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
