using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject playButton;
    public GameObject controlPanel;
    public GameObject progressLabel;
    public Text playersOnlineText;
    public  Object SceneObject;
    public GameObject easyModeToggle;
    public bool isPrivateGame;
    private bool readyToConnect;
    public byte MaxPlayersPerRoom = 16;
    private RoomOptions roomOptions = new RoomOptions();
    

    public override void OnConnectedToMaster()
    {
        readyToConnect = true;
        playersOnlineText.text = "Players Online: " + PhotonNetwork.CountOfPlayersInRooms.ToString() + "/16";
        controlPanel.SetActive(true);
     //   progressLabel.SetActive(false);
        playButton.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        
        progressLabel.SetActive(true);
        progressLabel.GetComponentInChildren<Text>().text = "Connection Lost. Attempting to reconnect.";
   //     controlPanel.SetActive(false);
        PhotonNetwork.Reconnect();
    }

    public override void OnJoinedRoom()
    {
        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.automaticallySyncScene to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {

            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel("Jungle");
        }
        
    }

    void Awake()
    {
        DontDestroyOnLoad(progressLabel);
        DontDestroyOnLoad(this);
        PhotonNetwork.SendRate = 50;
        PhotonNetwork.SerializationRate = 25;
        PhotonNetwork.GameVersion = "1.3.0";
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    void Start()
    {
        if(PlayerPrefs.GetInt("EasyModeEnabled") == 1)
        {
            easyModeToggle.GetComponent<Toggle>().isOn = true;
        }
        roomOptions.MaxPlayers = 16;
     //   progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        if (isPrivateGame)
        {
            int randomRoom = Random.Range(1000, 2000);
            PhotonNetwork.JoinOrCreateRoom("Room " + randomRoom.ToString(), roomOptions, null); //new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }
            progressLabel.SetActive(true);
            progressLabel.GetComponentInChildren<Text>().text = "Connecting... Please Wait.";
            controlPanel.SetActive(false);
            return;
        }
        if (PhotonNetwork.IsConnected && readyToConnect)
        {
            PhotonNetwork.JoinOrCreateRoom("Room One", roomOptions, null); //new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }
            progressLabel.SetActive(true);
            progressLabel.GetComponentInChildren<Text>().text = "Connecting... Please Wait.";
            controlPanel.SetActive(false);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void SetGamePrivate(bool value)
    {
        isPrivateGame = value;
    }

    public void EnableEasyMode(bool value)
    {
        int tempNum = 0;
        if (value == true)
        {
            tempNum = 1;
        }
        else
        {
            tempNum = 0;
        }
        PlayerPrefs.SetInt("EasyModeEnabled", tempNum);
    }

}