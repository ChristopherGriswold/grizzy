using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    static public GameManager Instance;
    public GameObject playerPrefab;
    public Transform playerSpawn;
    public List<PlayerInfo> playerInfos = new List<PlayerInfo>();

    private GameObject localPlayer;

    private void Awake()
    {
   //     DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Instance = this;
        if (PlayerManager.LocalPlayerInstance == null)
        {
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            localPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, playerSpawn.position, Quaternion.identity, 0);
        }
        else
        {
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        if (PhotonNetwork.IsMasterClient)
        {
            RecalculateSendRates();
                                                                                                 //   LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GameObject.Find("LocalPlayer").GetComponent<ChatManager>().LocalNotification(otherPlayer.NickName + ": Left the game", Color.red, true);
        if (PhotonNetwork.IsMasterClient)
        {
            RecalculateSendRates();
        }
    }



    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // app moved to background
          //  PhotonNetwork.LeaveRoom();
        }
        else
        {
            // app is foreground again
         //   PhotonNetwork.LeaveRoom();

          //  PhotonNetwork.ReconnectAndRejoin();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        localPlayer.GetComponent<PlayerVariables>().GetPosition();
        localPlayer.GetComponent<PlayerVariables>().GetBackpack();
        SceneManager.LoadScene(0);
        //  PhotonNetwork.LoadLevel("Launcher");
    }

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
            
    }

    

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void RecalculateSendRates()
    {
        int playerCount = PhotonNetwork.CountOfPlayersInRooms;
        if(playerCount < 2)
        {
            playerCount = 2;
        }
        PhotonNetwork.SendRate = (int)(60 / playerCount);
        PhotonNetwork.SerializationRate = (int)(30 / playerCount);
    }
}

public class PlayerInfo
{
    public string playerName;
    public int playerViewId;
}