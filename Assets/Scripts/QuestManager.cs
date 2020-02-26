using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class QuestManager : MonoBehaviour
{
    public static QuestManager ins;
    private void Awake()
    {
        ins = this;
        StartCoroutine(LinkPlayerBackpack());
    }

    public DialogueManager dialogueManager;
    public string npcName;
    public List<MiniQuest> miniQuests = new List<MiniQuest>();

    private Backpack backpack;
    private ChatManager chatManager;
    private GameObject player;
    private MiniQuestWWrapper wrappedMiniQuests;


    public bool CheckConditions()
    {
        for(int i = 0; i < miniQuests.Count; i++)
        {
            if(miniQuests[i].started)
            {
                for (int j = 0; j < miniQuests[i].items.Count; j++)
                {
                    miniQuests[i].conditionsMet = true;
                    if (!backpack.FindInBackpack(miniQuests[i].items[j]))
                    {
                        miniQuests[i].conditionsMet = false;
                    }
                }
                return miniQuests[i].conditionsMet;
            }else
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckConditions(int questId)
    {
        if (miniQuests[questId].started)
        {
            miniQuests[questId].conditionsMet = true;
            for (int i = 0; i < miniQuests[questId].items.Count; i++)
            {
                if (!backpack.FindInBackpack(miniQuests[questId].items[i]))
                {
                    miniQuests[questId].conditionsMet = false;
                }
            }
            return miniQuests[questId].conditionsMet;
        }
        else
        {
            return false;
        }
    }

    public bool CheckIfCompleted(int triggerIndex)
    {
        for (int i = 0; i < miniQuests.Count; i++)
        {
            if (miniQuests[i].completed)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveQuestItems(int miniQuestIndex)
    {
        if (miniQuests[miniQuestIndex].completed)
        {
            for(int i = 0; i < miniQuests[miniQuestIndex].items.Count; i++)
            {
                backpack.RemoveItem(miniQuests[miniQuestIndex].items[i]);
                chatManager.LocalNotification("Removed: " + miniQuests[miniQuestIndex].items[i], Color.red, true);
            }
        }
    }

    public void AddQuestItem(int miniQuestIndex)
    {
        if (miniQuests[miniQuestIndex].completed)
        {
            for (int i = 0; i < miniQuests[miniQuestIndex].rewards.Count; i++)
            {
                GameObject questReward = PhotonNetwork.Instantiate(miniQuests[miniQuestIndex].rewards[i], this.gameObject.transform.position, this.gameObject.transform.rotation, 0, null);
                questReward.GetComponent<ItemHandler>().ReceiveItem(chatManager.gameObject);
                chatManager.LocalNotification("Received: " + miniQuests[miniQuestIndex].rewards[i], Color.cyan, true);
            }
        }
    }

    public void TakeMoney(int miniQuestIndex)
    {
        player.GetComponent<PlayerVariables>().RemoveCash(50);
        chatManager.LocalNotification("Gold -" + miniQuests[miniQuestIndex].costGold, Color.red, true);
    }

    public void StartMiniQuest(int id)
    {
        for(int i = 0; i < miniQuests.Count; i++)
        {
            if(id == miniQuests[i].id)
            {
                miniQuests[i].started = true;
                chatManager.LocalNotification("Started Quest: " + miniQuests[i].name, Color.yellow, true);
            }
        }
    }

    public void CompleteMiniQuest(int id)
    {
        for (int i = 0; i < miniQuests.Count; i++)
        {
            if (id == miniQuests[i].id)
            {
                miniQuests[i].completed = true;
            //    RemoveQuestItems(i);
            if(chatManager != null)
                {

                    chatManager.LocalNotification("Quest Complete: " + miniQuests[i].name, Color.green, true);
                }
           //     AddQuestItem(i);
            }
        }
    }

    private void Start()
    {
        LoadMiniQuestData();
        //   SaveMiniQuestData();
    }

    IEnumerator LinkPlayerBackpack()
    {
        while (true)
        {
            try
            {
                player = GameObject.Find("LocalPlayer");
                
                if (player != null)
                {
                    backpack = player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>();
                    chatManager = player.GetComponent<ChatManager>();
                    StartMiniQuest(1);
                    yield break;
                }
            }
            catch
            {

            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void SaveMiniQuestData()
    {
        wrappedMiniQuests = new MiniQuestWWrapper(miniQuests);
        string contents = JsonUtility.ToJson(wrappedMiniQuests, true);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/Json/" + npcName + "_miniQuests.json", contents);
    }

    public void LoadMiniQuestData()
    {
        var _path = Application.streamingAssetsPath +"/" + npcName + "_miniQuests.json";
        UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(_path);
        www.SendWebRequest();
        while (!www.isDone)
        {
        }
        string jsonData = www.downloadHandler.text;

     //   string jsonData = File.ReadAllText(Application.dataPath + "/StreamingAssets/Json/" + npcName + "_miniQuests.json");
        wrappedMiniQuests = JsonUtility.FromJson<MiniQuestWWrapper>(jsonData);
        miniQuests = wrappedMiniQuests.miniQuests;
    }
}



[System.Serializable]
public class MiniQuest
{
    public string pointOfContact;
    public string name;
    public int id;
    public bool started;
    public bool conditionsMet;
    public bool completed;
    public int costGold;
    public List<string> items = new List<string>();
    public List<string> rewards = new List<string>();
}

[System.Serializable]
public class MiniQuestWWrapper
{
    public List<MiniQuest> miniQuests;

    public MiniQuestWWrapper(List<MiniQuest> miniQuestList)
    {
        miniQuests = miniQuestList;
    }
}
