using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMiniMap : MonoBehaviour {
    public GameObject previousMiniMap;
    public GameObject nextMiniMap;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            previousMiniMap.SetActive(false);
            nextMiniMap.SetActive(true);
        }
    }
}
