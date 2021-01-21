using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookable : MonoBehaviour
{

    public string cookedPrefabName;
    public AudioClip cookSound;
    public int levelToCook;
    public int timeToCook;
    public int xpReward;

    private GameObject player;
    private DataHandler dataHandler;
    private XPController xpController;
    private ItemHandler itemHandler;
    private ChatManager chatManager;

    public void Cook()
    {
        itemHandler = gameObject.GetComponent<ItemHandler>();
        int amount = itemHandler.amount;
        int slot = itemHandler.inSlot.slotId;
        player = transform.root.gameObject;
        chatManager = player.GetComponent<ChatManager>();
        dataHandler = player.GetComponent<DataHandler>();
        xpController = player.GetComponent<XPController>();
        if (Mathf.Pow(dataHandler.playerData.cookingXp, .25f) >= levelToCook)
        {
            xpController.GainXp("Cooking", xpReward);
            dataHandler.playerData.items[slot] = cookedPrefabName;
            dataHandler.playerData.itemAmounts[slot] = amount;
            chatManager.LocalNotification("Cooked: " + cookedPrefabName, Color.green, true);
         //   player.GetComponentInChildren<Backpack>(true).SetBackpack();
            Destroy(this.gameObject);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
