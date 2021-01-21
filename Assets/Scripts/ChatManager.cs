using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class ChatManager : MonoBehaviour
{

    public int maxMessages = 25;
    public GameObject chatMessagePrefab;
    public GameObject chatBubblePrefab;
    public string playerName;
    public Vector3 playerPosition;
    public GameObject chatPanel;
    private PlayerVariables playerVariables;

    public GameObject arrowPrefab; // FIXME THIS IS BOGUS!


    [SerializeField]
    List<GameObject> messageList = new List<GameObject>();

    private void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName");
        SendChatMessageGlobal(playerName + ": Joined the game");
        playerVariables = chatPanel.transform.root.GetComponent<PlayerVariables>();
    }

    public void SendChatMessage(string text)
    {
        if(text != "")
        {
            if (messageList.Count >= maxMessages)
            {
                Destroy(messageList[0].gameObject);
                messageList.Remove(messageList[0]);
            }
            Vector3 chatPanelPos = chatPanel.GetComponent<RectTransform>().localPosition;
            chatPanelPos.y = 0;
            chatPanel.GetComponent<RectTransform>().localPosition = chatPanelPos;
            GameObject chatMessage = Instantiate(chatMessagePrefab, chatPanel.transform);
            chatMessage.transform.localPosition = Vector3.zero;
            chatMessage.transform.localRotation = Quaternion.identity;
            chatMessage.GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<PlayerVariables>().playerName + ": " + text;
            //  chatPanel = GameObject.Find("Content").transform;
            GetComponentInChildren<ConsolePanel>().StartDisplay();
            messageList.Add(chatMessage);
            playerName = gameObject.GetComponent<PlayerVariables>().playerName;
            playerPosition = gameObject.transform.position;
            RpcSendChatMessage(text, playerPosition);
        }
    }

    public void SendChatMessageGlobal(string text)
    {
        Vector3 chatPanelPos = chatPanel.GetComponent<RectTransform>().localPosition;
        chatPanelPos.y = 0;
        chatPanel.GetComponent<RectTransform>().localPosition = chatPanelPos;
        GameObject chatMessage = Instantiate(chatMessagePrefab, chatPanel.transform);
        chatMessage.transform.localPosition = Vector3.zero;
        chatMessage.transform.localRotation = Quaternion.identity;
        chatMessage.GetComponent<TextMeshProUGUI>().text = text;
            chatMessage.GetComponent<TextMeshProUGUI>().color = Color.yellow;
        //    chatPanel = GameObject.Find("Content");
            GetComponentInChildren<ConsolePanel>().StartDisplay();
            messageList.Add(chatMessage);
        //    RpcSendChatMessageGlobal(text);
    }

    void RpcSendChatMessage(string text, Vector3 playerPos)
    {
        bool cheated = false;
        if(text == "MoreMoney")
        {
            GameObject.Find("Player").GetComponent<DataHandler>().playerData.cash += 2000;
            text = "Cheat activated";
            cheated = true;
        }else if(text == "MoreHealth")
        {
            GameObject.Find("Player").GetComponentInChildren<HealthController>().ReplenishHealth(100);
            text = "Cheat activated";
            cheated = true;
        }
        else if (text == "MoreArrows")
        {
            GameObject arrow = Instantiate(arrowPrefab);
            GameObject.Find("Player").GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().AddToBackpack(arrow);

            arrow.GetComponent<ItemHandler>().SetAmount(100);
            text = "Cheat activated";
            cheated = true;
        }
        else if (text == "MoreXp")
        {
            GameObject.Find("Player").GetComponent<XPController>().GainXp("Attack", 5000);
            text = "Cheat activated";
            cheated = true;
        }
        else if (text == "Suicide")
        {
            GameObject.Find("Player").GetComponentInChildren<HealthController>().LoseHealth(100);
            text = "Cheat activated";
            cheated = true;
        }
       
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].gameObject);
            messageList.Remove(messageList[0]);
        }

        if (cheated)
        {
            GameObject.Find("Player").GetComponent<PlayerData>().cheated = true;
        }
        GameObject chatMessage = Instantiate(chatMessagePrefab);
        chatMessage.GetComponent<TextMeshProUGUI>().text = text;
        chatMessage.GetComponent<TextMeshProUGUI>().color = Color.green;
        chatPanel = GameObject.Find("Content");
        chatMessage.transform.SetParent(chatPanel.transform);
        GetComponentInChildren<ConsolePanel>().StartDisplay();
        messageList.Add(chatMessage);




        //      if (Vector3.Distance(GameObject.Find("Player").transform.position, playerPos) < 25)
        if (false)
            {
                if (text != "")
                {
                    if (messageList.Count >= maxMessages)
                    {
                        Destroy(messageList[0].gameObject);
                        messageList.Remove(messageList[0]);
                    }
             //       GameObject chatMessage = Instantiate(chatMessagePrefab);
                    chatMessage.GetComponent<TextMeshProUGUI>().text = text;
                    chatMessage.GetComponent<TextMeshProUGUI>().color = Color.green;
                    chatPanel = GameObject.Find("Content");
                    chatMessage.transform.SetParent(chatPanel.transform);
                    GetComponentInChildren<ConsolePanel>().StartDisplay();
                    messageList.Add(chatMessage);
                }
            }
    }

    void RpcSendChatMessageGlobal(string text)
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




    IEnumerator ClearBubbleText()
    {
        yield return new WaitForSeconds(5f);
    }

    public void LocalNotification(GameObject g)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].gameObject);
            messageList.Remove(messageList[0]);
        }
        Vector3 chatPanelPos = chatPanel.GetComponent<RectTransform>().localPosition;
        chatPanelPos.y = 0;
        chatPanel.GetComponent<RectTransform>().localPosition = chatPanelPos;
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
        Vector3 chatPanelPos = chatPanel.GetComponent<RectTransform>().localPosition;
        chatPanelPos.y = 0;
        chatPanel.GetComponent<RectTransform>().localPosition = chatPanelPos;
        GameObject chatMessage = Instantiate(chatMessagePrefab, chatPanel.transform);
        chatMessage.transform.localPosition = Vector3.zero;
        chatMessage.transform.localRotation = Quaternion.identity;
        chatMessage.GetComponent<TextMeshProUGUI>().text = s;
        chatMessage.GetComponent<TextMeshProUGUI>().color = Color.red;
        chatPanel = GameObject.Find("Content");
        chatMessage.transform.SetParent(chatPanel.transform);
        GetComponentInChildren<ConsolePanel>().StartDisplay();
        messageList.Add(chatMessage);
        StartCoroutine(ClearNotification(chatMessage));
    }
    public void LocalNotification(string s, Color color, bool dontClear)
    {
        Vector3 chatPanelPos = chatPanel.GetComponent<RectTransform>().localPosition;
        chatPanelPos.y = 0;
        chatPanel.GetComponent<RectTransform>().localPosition = chatPanelPos;
        GameObject chatMessage = Instantiate(chatMessagePrefab, chatPanel.transform);
        chatMessage.transform.localPosition = Vector3.zero;
        chatMessage.transform.localRotation = Quaternion.identity;
        chatMessage.GetComponent<TextMeshProUGUI>().text = s;
        chatMessage.GetComponent<TextMeshProUGUI>().color = color;
      //  chatPanel = GameObject.Find("Content").transform;
    //    chatMessage.transform.SetParent(chatPanel.transform);
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