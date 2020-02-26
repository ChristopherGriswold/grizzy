using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPun, IPunObservable
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    public GameObject hiddenPlayerObjects;
    public PlayerController playerController;
    public ChatManager chatManager;
    public GameObject weapon;

    void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
      //  DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {
        
        if (photonView.IsMine)
        {
            hiddenPlayerObjects.SetActive(true);
            playerController.enabled = true;
            chatManager.enabled = true;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.name == "SECRET")
        {
            PhotonNetwork.LoadLevel("Other Area");
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
         //   stream.SendNext(Health);
        }
        else
        {
            // Network player, receive data
         //   this.Health = (float)stream.ReceiveNext();
        }
    }
}
