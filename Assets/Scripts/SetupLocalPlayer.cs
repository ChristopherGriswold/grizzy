using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;

public class SetupLocalPlayer : MonoBehaviourPun
{
    public GameObject visor;
    public GameObject blip;
    public GameObject chatBubble;

    void Start()
    {
        if (photonView.IsMine)
        {
            gameObject.name = "LocalPlayer";
            GetComponent<PlayerController>().enabled = true;
            visor.SetActive(false);
            chatBubble.SetActive(false);
        }
        else
        {
            gameObject.name = "Player " + Random.Range(2, 20);
            MoveToLayer(gameObject.transform, 17);
            blip.GetComponent<SpriteRenderer>().color = Color.blue;
            blip.layer = 11;
        }
	}

    void MoveToLayer(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
        {
            MoveToLayer(child, layer);
        }
    }
}
