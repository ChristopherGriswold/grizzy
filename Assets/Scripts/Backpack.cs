using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public List<GameObject> itemsInBackpack = new List<GameObject>();
    public GameObject[] slot = new GameObject[16];
    public GameObject[] equipmentSlot = new GameObject[2];
    public GameObject heldItem;

    public bool AddToBackpack(GameObject item)
    {
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].GetComponent<Slot>().isFilled)
            {
                if (slot[i].GetComponent<Slot>().item.name == item.name && item.GetComponent<ItemHandler>().isStackable)
                {
                    slot[i].GetComponent<Slot>().AddToItem(item.GetComponent<ItemHandler>().amount);
                  //  itemDatabase.RemoveItemFromDatabase(item);
                    Destroy(item);
                    return true;
                }
            }
        }

        for (int i = 0; i < slot.Length; i++)
        {
            if (!slot[i].GetComponent<Slot>().isFilled)
            {
                slot[i].GetComponent<Slot>().FillSlot(item);
                itemsInBackpack.Add(item);
                PlayerPrefs.SetString("Slot " + i, item.name);
                return true;
            }
        }
        return false;
    }

    public GameObject FindInBackpack(GameObject g)
    {
        GameObject foundItem = null;
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].GetComponent<Slot>().isFilled)
            {
                if (g.GetComponent<ItemHandler>().combinationsList[0].combinesWith == (slot[i].GetComponent<Slot>().item.name))//slot[i].GetComponent<Slot>().item.name == g.GetComponent<ItemHandler>().combinesWith)
                {
                    foundItem = slot[i].GetComponent<Slot>().item;
                }
            }
        }
        return foundItem;
    }

    public bool FindInBackpack(string name)
    {
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].GetComponent<Slot>().isFilled)
            {
                if (slot[i].GetComponent<Slot>().item.name == name)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public GameObject FindInBackpack(string name, bool returnObject)
    {
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].GetComponent<Slot>().isFilled)
            {
                if (slot[i].GetComponent<Slot>().item.name == name)
                {
                    return slot[i].GetComponent<Slot>().item;
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
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].GetComponent<Slot>().isFilled)
            {
                if (slot[i].GetComponent<Slot>().item.name == g.name)
                {
                    slot[i].GetComponent<Slot>().DeselectSlot();
                    slot[i].GetComponent<Slot>().EmptySLot();
                    itemsInBackpack.Remove(g);
                    return;
                }
            }
        }
          
    }

    public void RemoveItem(string name)
    {
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].GetComponent<Slot>().isFilled)
            {
                if (slot[i].GetComponent<Slot>().item.name == name)
                {
                    ConsumeItem(slot[i].GetComponent<Slot>().item);
                    slot[i].GetComponent<Slot>().DeselectSlot();
                    slot[i].GetComponent<Slot>().EmptySLot();
                    return;
                }
            }
        }
    }

    public bool RoomInBackpack()
    {
        for (int i = 0; i < slot.Length; i++)
        {
            if (!slot[i].GetComponent<Slot>().isFilled)
            {
                return true;
            }
        }
        return false;
    }

    public bool RoomInBackpack(GameObject item)
    {
        for (int i = 0; i < slot.Length; i++)
        {
            if (!slot[i].GetComponent<Slot>().isFilled)
            {
                return true;
            }
            else if(slot[i].GetComponent<Slot>().item.name == item.name && item.GetComponent<ItemHandler>().isStackable)
            {
                return true;
            }
        }
        return false;
    }
}