using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ItemDropper : MonoBehaviour
{

    public GameObject[] itemsToDrop = new GameObject[5];
    // Start is called before the first frame update
    private void Awake()
    {
        foreach (GameObject item in itemsToDrop)
        {
            GameObject newItem = PhotonNetwork.Instantiate(item.name, this.gameObject.transform.position, this.gameObject.transform.rotation, 0, null);
        }
    }
}
