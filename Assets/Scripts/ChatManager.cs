using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Photon.Pun;

public class ChatManager : MonoBehaviourPun
{

    public int maxMessages = 25;
    public GameObject chatMessagePrefab;
    public GameObject chatBubblePrefab;
    public string playerName;
    public Vector3 playerPosition;
    private GameObject chatPanel;
    private PlayerVariables playerVariables;
    public TextMeshPro bubbleTextMesh;


    [SerializeField]
    List<GameObject> messageList = new List<GameObject>();

    private void OnEnable()
    {
        chatPanel = GameObject.Find("Content");
        playerName = PlayerPrefs.GetString("PlayerName");
        SendChatMessageGlobal(playerName + ": Joined the game");
        playerVariables = chatPanel.transform.root.GetComponent<PlayerVariables>();
        playerVariables.SetBackpack();
    }

    public void SendChatMessage(string text)
    {
        if(text != "")
        {
            if (photonView.IsMine)
            {
                if (messageList.Count >= maxMessages)
                {
                    Destroy(messageList[0].gameObject);
                    messageList.Remove(messageList[0]);
                }
                GameObject chatMessage = Instantiate(chatMessagePrefab);
                chatMessage.GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<PlayerVariables>().playerName + ": " + text;
              //  chatPanel = GameObject.Find("Content").transform;
                chatMessage.transform.SetParent(chatPanel.transform);
                GetComponentInChildren<ConsolePanel>().StartDisplay();
                messageList.Add(chatMessage);
            }
            playerName = gameObject.GetComponent<PlayerVariables>().playerName;
            playerPosition = gameObject.transform.position;
            photonView.RPC("RpcSendChatMessage", RpcTarget.All, playerName + ": " + text, playerPosition);
        }
    }

    public void SendChatMessageGlobal(string text)
    {
        if (photonView.IsMine)
        {
            GameObject chatMessage = Instantiate(chatMessagePrefab);
            chatMessage.GetComponent<TextMeshProUGUI>().text = text;
            chatMessage.GetComponent<TextMeshProUGUI>().color = Color.yellow;
        //    chatPanel = GameObject.Find("Content");
            chatMessage.transform.SetParent(chatPanel.transform);
            GetComponentInChildren<ConsolePanel>().StartDisplay();
            messageList.Add(chatMessage);
        }
        photonView.RPC("RpcSendChatMessageGlobal", RpcTarget.All, text);
    }

    [PunRPC]
    void RpcSendChatMessage(string text, Vector3 playerPos)
    {
        if (!photonView.IsMine)
        {
            if (Vector3.Distance(GameObject.Find("LocalPlayer").transform.position, playerPos) < 25)
            {
                if (text != "")
                {
                    if (messageList.Count >= maxMessages)
                    {
                        Destroy(messageList[0].gameObject);
                        messageList.Remove(messageList[0]);
                    }
                    GameObject chatMessage = Instantiate(chatMessagePrefab);
                    chatMessage.GetComponent<TextMeshProUGUI>().text = text;
                    chatMessage.GetComponent<TextMeshProUGUI>().color = Color.green;
                    chatPanel = GameObject.Find("Content");
                    chatMessage.transform.SetParent(chatPanel.transform);
                    GetComponentInChildren<ConsolePanel>().StartDisplay();
                    messageList.Add(chatMessage);
                }
            }
        }
    }

    [PunRPC]
    void RpcSendChatMessageGlobal(string text)
    {
        if (!photonView.IsMine)
        {
            if (messageList.Count >= maxMessages)
            {
                Destroy(messageList[0].gameObject);
                messageList.Remove(messageList[0]);
            }
            GameObject chatMessage = Instantiate(chatMessagePrefab);
            chatMessage.GetComponent<TextMeshProUGUI>().text = text;
            chatMessage.GetComponent<TextMeshProUGUI>().color = Color.yellow;
            chatPanel = GameObject.Find("Content");
            chatMessage.transform.SetParent(chatPanel.transform);
            GetComponentInChildren<ConsolePanel>().StartDisplay();
            messageList.Add(chatMessage);
        }
    }
    public void ChangeChatBubbleText(string text)
    {
        if(text != "")
        {
            photonView.RPC("RpcChangeChatBubbleText", RpcTarget.All, text, photonView.ViewID);
        }
    }

    [PunRPC]
    void RpcChangeChatBubbleText(string text, int viewId)
    {
        if (photonView.IsMine)
        {
            return;
        }
        PostBubbleText(text, viewId);
    }

    void PostBubbleText(string text, int viewId)
    {
        bubbleTextMesh =   PhotonView.Find(viewId).gameObject.GetComponentInChildren<TextMeshPro>();
        bubbleTextMesh.text = text;
        StopCoroutine("ClearBubbleText");
        StartCoroutine("ClearBubbleText");
    }

    IEnumerator ClearBubbleText()
    {
        yield return new WaitForSeconds(5f);
        bubbleTextMesh.text = "";
    }

    public void LocalNotification(GameObject g)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].gameObject);
            messageList.Remove(messageList[0]);
        }
        GameObject chatMessage = Instantiate(chatMessagePrefab);
        chatMessage.GetComponent<TextMeshProUGUI>().text = "Picked up: " + g.name;
        chatMessage.GetComponent<TextMeshProUGUI>().color = Color.white;
      //  chatPanel = GameObject.Find("Content").transform;
        chatMessage.transform.SetParent(chatPanel.transform);
        GetComponentInChildren<ConsolePanel>().StartDisplay();
        messageList.Add(chatMessage);
    }

    public void LocalNotification(string s)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        GameObject chatMessage = Instantiate(chatMessagePrefab);
        chatMessage.GetComponent<TextMeshProUGUI>().text = s;
        chatMessage.GetComponent<TextMeshProUGUI>().color = Color.red;
      //  chatPanel = GameObject.Find("Content").transform;
        chatMessage.transform.SetParent(chatPanel.transform);
        GetComponentInChildren<ConsolePanel>().StartDisplay();
        messageList.Add(chatMessage);
        StartCoroutine(ClearNotification(chatMessage));
    }
    public void LocalNotification(string s, Color color, bool dontClear)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        GameObject chatMessage = Instantiate(chatMessagePrefab);
        chatMessage.GetComponent<TextMeshProUGUI>().text = s;
        chatMessage.GetComponent<TextMeshProUGUI>().color = color;
      //  chatPanel = GameObject.Find("Content").transform;
        chatMessage.transform.SetParent(chatPanel.transform);
        GetComponentInChildren<ConsolePanel>().StartDisplay();
        messageList.Add(chatMessage);
        if (!dontClear)
        {
            StartCoroutine(ClearNotification(chatMessage));
        }
    }

    public IEnumerator ClearNotification(GameObject g)
    {
        yield return new WaitForSeconds(5f);
        Destroy(g);
        messageList.Remove(g);
    }


}