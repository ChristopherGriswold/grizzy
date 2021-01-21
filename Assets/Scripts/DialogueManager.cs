using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager ins;
    private void Awake()
    {
        ins = this;
    }
    public string npcName;

    public QuestManager questManager;
    public List<DialogueTrigger> triggers = new List<DialogueTrigger>();
    public List<Node> nodes = new List<Node>();
    private int nodeIndex;

    private GameObject[] buttons = new GameObject[4];
    private GameObject customTouchPad;
    private GameObject nameField;
    private GameObject textField;
    private bool actionIsPending;
    private bool panelIsLinked;
    private int nextNodeId;
    private int questConditionsMetEntryNodeId;
    private int startNodeId = 0;
    private bool shouldRegress;
    private bool questConditionsMet;
    private NodeWrapper nodeWrapper;
    private DialogueTriggerWrapper dialogueTriggerWrapper;


    private void Start()
    {
        LoadNodes();
      //  LoadTriggers();
        //    SaveNodes();
        //   SaveTriggers();
    }

    public void SaveNodes()
    {
        nodeWrapper = new NodeWrapper(nodes);
        string contents = JsonUtility.ToJson(nodeWrapper, true);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/Json/" + npcName + "_dlg.json", contents);
    }

    public void LoadNodes()
    {
        var _path = Application.streamingAssetsPath + "/" + npcName + "_dlg.json";
        UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(_path);
        www.SendWebRequest();
        while (!www.isDone)
        {
        }
        string jsonData = www.downloadHandler.text;
     //   string jsonData = File.ReadAllText(Application.dataPath + "/StreamingAssets/Json/" + npcName + "_dlg.json");
        nodeWrapper = JsonUtility.FromJson<NodeWrapper>(jsonData);
        nodes = nodeWrapper.wrappedNodes;

    }

    public void SaveTriggers()
    {
        dialogueTriggerWrapper = new DialogueTriggerWrapper(triggers);
        string contents = JsonUtility.ToJson(dialogueTriggerWrapper, true);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/Json/" + npcName + "_dlg_trig.json", contents);
    }

    public void LoadTriggers()
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/StreamingAssets/Json/" + npcName + "_dlg_trig.json");
        dialogueTriggerWrapper = JsonUtility.FromJson<DialogueTriggerWrapper>(jsonData);
        triggers = dialogueTriggerWrapper.wrappedTriggers;

    }

    public void PopulateDialogue(string ok)
    {
        StopAllCoroutines();
        buttons[0].SetActive(false);
        buttons[1].SetActive(false);
        buttons[2].SetActive(false);
        buttons[3].SetActive(false);

       
        nameField.GetComponent<TextMeshProUGUI>().text = nodes[nextNodeId].who;
        if (nextNodeId == 0)
        {
            textField.GetComponent<TextMeshProUGUI>().text = nodes[nextNodeId].text;
        }
        else
        {
            StartCoroutine(TypeSentence(nodes[nextNodeId].text));
        }
        for (int i = 0; i < nodes[nextNodeId].options.Count; i++)
        {
            int i2 = i;
            buttons[i].SetActive(true);
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = nodes[nextNodeId].options[i].text;
            buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            buttons[i].GetComponent<Button>().onClick.AddListener(delegate { ButtonClicked(nextNodeId, i2); });
        }
    }

    public void PopulateDialogue(bool firstContact)
    {
        CheckTrigger();
       
        if (firstContact)
        {
            nodeIndex = FindNodeIndexWithId(startNodeId);
        }
        if (questConditionsMet)
        {
            nodeIndex = FindNodeIndexWithId(questConditionsMetEntryNodeId);
        }
        StopAllCoroutines();
        buttons[0].SetActive(false);
        buttons[1].SetActive(false);
        buttons[2].SetActive(false);
        buttons[3].SetActive(false);
        nameField.GetComponent<TextMeshProUGUI>().text = nodes[nodeIndex].who;
        if (nodeIndex == 0)
        {
            textField.GetComponent<TextMeshProUGUI>().text = nodes[nodeIndex].text;
        }
        else
        {
            StartCoroutine(TypeSentence(nodes[nodeIndex].text));
        }
        for (int i = 0; i < nodes[nodeIndex].options.Count; i++)
        {
            int i2 = i;
            buttons[i].SetActive(true);
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = nodes[nodeIndex].options[i].text;
            buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            buttons[i].GetComponent<Button>().onClick.AddListener(delegate { ButtonClicked(nodeIndex, i2); });
        }
    }


    public void LinkPanel(GameObject panel, GameObject touchpad)
    {
        if (!panelIsLinked)
        {
            customTouchPad = touchpad;
            nameField = panel.GetComponent<DialoguePanelRefs>().nameObject;
            textField = panel.GetComponent<DialoguePanelRefs>().textObject;
            buttons[0] = panel.GetComponent<DialoguePanelRefs>().button0;
            buttons[1] = panel.GetComponent<DialoguePanelRefs>().button1;
            buttons[2] = panel.GetComponent<DialoguePanelRefs>().button2;
            buttons[3] = panel.GetComponent<DialoguePanelRefs>().button3;
            panelIsLinked = true;
        }
    }

    public void RegressDialogue(int actionId)
    {
        if (triggers[actionId].offNode != -1)
        {
         //   nodes[triggers[actionId].offNode].isAccessible = true;
        }
        if (triggers[actionId].onNode != -1)
        {
        //    nodes[triggers[actionId].onNode].isAccessible = false;
        }
        nodes[0].options[0].destinationId = triggers[actionId].offNode;
        triggers[actionId].isActive = true;
    }

    public void ButtonClicked(int nodeId, int optionIndex)
    {
        nextNodeId = nodes[nodeId].options[optionIndex].destinationId;
        CheckTrigger(nodes[nodeId].options[optionIndex]);
        if (nodes[nodeId].options[optionIndex].destinationId == -1)
        {
            customTouchPad.GetComponent<CustomTouchPad>().ClearTarget();
            return;
        }
        
        nodeIndex = FindNodeIndexWithId(nextNodeId);
        PopulateDialogue(false);
    }


    public IEnumerator TypeSentence(string sentence)
    {
        StringBuilder builder = new StringBuilder();
        foreach (char letter in sentence.ToCharArray())
        {
            builder.Append(letter);
            textField.GetComponent<TextMeshProUGUI>().text = builder.ToString();
            yield return new WaitForSeconds(.02f);
        }
    }

    private Trigger CheckTrigger(Option nodeOption)
    {
        for (int i = 0; i < nodeOption.triggers.Count; i++)
        {
            ActivateTrigger(nodeOption.triggers[i]);
        }
        return null;
    }

    private void CheckTrigger()
    {
        for(int i = 0; i < questManager.miniQuests.Count-1; i++) /////BIG PROBLEM HERE WITH THE MINUS 1!!!!
        {
            if (questManager.miniQuests[i].pointOfContact == this.gameObject.GetComponent<NpcController>().npcName)
            {
                if (questManager.CheckConditions(i) && !questManager.miniQuests[i].completed)
                {
                    questConditionsMet = true;
                }
                else
                {
                    questConditionsMet = false;
                }
            }
        }
    }


    public void ActivateTrigger(Trigger trig)
    {
        switch (trig.triggerType)
        {
            case "QuestStart":
                questManager.StartMiniQuest(trig.actionId);
                questConditionsMetEntryNodeId = trig.secondaryActionId;
                break;
            case "QuestComplete":
                questManager.CompleteMiniQuest(trig.actionId);
                break;
            case "ChangeEntryNode":
                startNodeId = trig.actionId;
                break;
            case "TakeItem":
                questManager.RemoveQuestItems(trig.actionId);
                break;
            case "TakeMoney":
                questManager.TakeMoney(trig.actionId);
                break;
            case "GiveItem":
                questManager.AddQuestItem(trig.actionId);
                break;
            case "ReplaceItem":
                break;
            case "ConditionMet":
                break;
            case "SetNodeDestinationId":
                nodes[FindNodeIndexWithId(trig.actionId)].options[trig.thirdActionId].destinationId = trig.secondaryActionId;
                break;
            default:
                break;
        }
    }

    private int FindNodeIndexWithId(int id)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if(nodes[i].id == id)
            {
                return i;
            }
        }
        return 0;
    }
}

[System.Serializable]
public class Node
{
    public int id;
    public string who;
    public string text;

    public List<Option> options = new List<Option>();
}

[System.Serializable]
public class Option
{
    public string text;
    public int destinationId;
    public List<Trigger> triggers = new List<Trigger>();
}

[System.Serializable]
public class Trigger
{
    public string triggerType;
    public int actionId;
    public int secondaryActionId;
    public int thirdActionId;
}


[System.Serializable]
public class DialogueTrigger
{
    public bool isActive;
    public bool conditionMet;
    public string name;
    public Vector2 nodeOptionId;
    public int offNode;
    public int onNode;
    public int newEntryId;
    public int startsQuestId;
    public int completesQuestId;
}

[System.Serializable]
public class DialogueTrigger2
{
    public bool isActive;
    public bool conditionMet;
    public string name;
    public int newEntryId;
    public int startsQuestId;
    public int completesQuestId;
}

[System.Serializable]
public class NodeWrapper
{
    public List<Node> wrappedNodes;

    public NodeWrapper(List<Node> wrappedNodesList)
    {
        wrappedNodes = wrappedNodesList;
    }
}

[System.Serializable]
public class DialogueTriggerWrapper
{
    public List<DialogueTrigger> wrappedTriggers;

    public DialogueTriggerWrapper(List<DialogueTrigger> wrappedTriggersList)
    {
        wrappedTriggers = wrappedTriggersList;
    }
}