using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public List<GameObject> itemsInBackpack = new List<GameObject>();
    public GameObject[] slots = new GameObject[18];
    private GameObject player;
    public GameObject menu;
    private DataHandler dataHandler;

    private PlayerData playerData;

    private void Awake()
    {
        PreLoadBackpack();
    }
    private void Start()
    {
        PreLoadBackpack();
        SetBackpack(true);
    }

    public void PreLoadBackpack()
    {
        player = this.gameObject.transform.root.gameObject;
        playerData = player.GetComponent<DataHandler>().playerData;
    }

    public void ClearBackpack()
    {
        for (int i = 0; i < playerData.items.Length; i++)
        {
            if (playerData.items[i] != "Empty")
            {
                InsertIntoBackpack(playerData.items[i], playerData.itemAmounts[i], i);
            }
            else
            {
                if (slots[i].GetComponent<Slot>().isFilled)
                {
                    ConsumeItem(slots[i].GetComponent<Slot>().item);
                    slots[i].GetComponent<Slot>().DeselectSlot();
                    slots[i].GetComponent<Slot>().EmptySLot();
                }
            }
        }
    }


    public void SetBackpack(bool includeEquipment)
    {
        int startPos;
        if (!includeEquipment)
        {
            startPos = 2;
        }
        else
        {
            startPos = 0;
        }
        for (int i = startPos; i < playerData.items.Length; i++)
        {
            if (playerData.items[i] != "Empty")
            {
                InsertIntoBackpack(playerData.items[i], playerData.itemAmounts[i], i);
            }
            else
            {
                if (slots[i].GetComponent<Slot>().isFilled)
                {
                    ConsumeItem(slots[i].GetComponent<Slot>().item);
                    slots[i].GetComponent<Slot>().DeselectSlot();
                    slots[i].GetComponent<Slot>().EmptySLot();
                }
            }
        }
        menu.SetActive(false);
    }

    public void InsertIntoBackpack(string itemName, int iamount, int inslot)
    {
        GameObject newObject = (GameObject)Instantiate(Resources.Load(itemName));
        ItemHandler itemHandler = newObject.GetComponent<ItemHandler>();
        itemHandler.amount = iamount;
        newObject.name = itemHandler.itemName;
        if (inslot != 0 || inslot != 1)
        {
            slots[inslot].GetComponent<Slot>().FillSlot(newObject);
        }
        itemsInBackpack.Add(newObject);

        itemHandler.triggerCollider.SetActive(false);
        player = this.gameObject.transform.root.gameObject;
        newObject.transform.SetParent(player.GetComponent<PlayerReferences>().itemDatabase.transform);
        newObject.SetActive(false);
        itemHandler.doGetGatheringXp = false;
        if (inslot == 0 || inslot == 1)
        {
          //  menu.SetActive(true);

            menu.GetComponentInChildren<UseButtonHandler>().selectedItemHandler = itemHandler;
            menu.GetComponentInChildren<UseButtonHandler>().weaponSlot.GetComponent<Slot>().isFilled = false;
            menu.GetComponentInChildren<UseButtonHandler>().Equip(newObject);
            
        }
    }

    public bool AddToBackpack(GameObject item)
    {
        for (int i = 2; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Slot>().isFilled)
            {
                if (slots[i].GetComponent<Slot>().item.name == item.name && item.GetComponent<ItemHandler>().isStackable)
                {
                    slots[i].GetComponent<Slot>().AddToItem(item.GetComponent<ItemHandler>().amount);
                    Destroy(item);
                    return true;
                }
            }
        }

        for (int i = 2; i < slots.Length; i++)
        {
            if (!slots[i].GetComponent<Slot>().isFilled)
            {
                slots[i].GetComponent<Slot>().FillSlot(item);
                itemsInBackpack.Add(item);

                playerData.items[i] = item.GetComponent<ItemHandler>().itemName;
                playerData.itemAmounts[i] = item.GetComponent<ItemHandler>().amount;
                return true;
            }
        }
        return false;
    }

    public GameObject FindInBackpack(GameObject g)
    {
        GameObject foundItem = null;
        for (int i = 2; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Slot>().isFilled)
            {
                if (g.GetComponent<ItemHandler>().combinationsList[0].combinesWith == (slots[i].GetComponent<Slot>().item.name))//slot[i].GetComponent<Slot>().item.name == g.GetComponent<ItemHandler>().combinesWith)
                {
                    foundItem = slots[i].GetComponent<Slot>().item;
                }
            }
        }
        return foundItem;
    }

    public bool FindInBackpack(string name)
    {
        for (int i = 2; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Slot>().isFilled)
            {
                if (slots[i].GetComponent<Slot>().item.name == name)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public GameObject FindInBackpack(string name, bool returnObject)
    {
        for (int i = 2; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Slot>().isFilled)
            {
                if (slots[i].GetComponent<Slot>().item.name == name)
                {
                    return slots[i].GetComponent<Slot>().item;
                }
            }
        }
        return null;
    }

    public void ConsumeItem(GameObject g)
    {
        itemsInBackpack.Remove(g);
        Destroy(g);
    }
    public void ReplaceItem(GameObject item1, GameObject item2)
    {
        itemsInBackpack.Remove(item1);
        itemsInBackpack.Add(item2);
    }
    public void RemoveItem(GameObject g)
    {
/*
        for (int i = 2; i < slot.Length; i++)
        {
            if (slot[i].GetComponent<Slot>().isFilled)
            {
                if (slot[i].GetComponent<Slot>().item.name == g.name)
                {
                    playerData.items[i] = "Empty";
                    playerData.itemAmounts[i] = 0;
                    slot[i].GetComponent<Slot>().DeselectSlot();
                    slot[i].GetComponent<Slot>().EmptySLot();
                    itemsInBackpack.Remove(g);
                    return;
                }
            }
        }*/
          
    }

    public void RemoveItem(string name)
    {
        for (int i = 2; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Slot>().isFilled)
            {
                if (slots[i].GetComponent<Slot>().item.name == name)
                {
                    ConsumeItem(slots[i].GetComponent<Slot>().item);
                    slots[i].GetComponent<Slot>().DeselectSlot();
                    slots[i].GetComponent<Slot>().EmptySLot();
                    return;
                }
            }
        }
    }

    public bool RoomInBackpack()
    {
        for (int i = 2; i < slots.Length; i++)
        {
            if (!slots[i].GetComponent<Slot>().isFilled)
            {
                return true;
            }
        }
        return false;
    }

    public bool RoomInBackpack(GameObject item)
    {
        for (int i = 2; i < slots.Length; i++)
        {
            if (!slots[i].GetComponent<Slot>().isFilled)
            {
                return true;
            }
            else if(slots[i].GetComponent<Slot>().item.name == item.name && item.GetComponent<ItemHandler>().isStackable)
            {
                return true;
            }
        }
        return false;
    }
}