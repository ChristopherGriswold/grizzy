using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItems : MonoBehaviour
{
    public List<GameObject> itemsInBackpack = new List<GameObject>();

    public void AddItemToBackpack(GameObject item)
    {
        if (!itemsInBackpack.Contains(item))
        {
            itemsInBackpack.Add(item);
            item.transform.SetParent(gameObject.transform);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            item.gameObject.SetActive(false);
        }
    }

    /*
    public bool IsItemInBackpack(GameObject item)
    {
        for (int i = 0; i < itemsInBackpack.Count; i++)
        {
            if (item.name == itemsInBackpack[i].name)
            {
                return true;
            }
        }
        return false;
    }
    */

    public bool IsItemInBackpack(string item)
    {
        for (int i = 0; i < itemsInBackpack.Count; i++)
        {
            if (item == itemsInBackpack[i].name)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItemFromBackpack(GameObject item)
    {
        itemsInBackpack.Remove(item);
    }

    /*
    public void RemoveItemFromDatabase(string itemName)
    {
        for(int i = 0; i < itemsInBackpack.Count; i++)
        {
            if(itemsInBackpack[i].name == itemName)
            {
                itemsInBackpack.Remove(itemsInBackpack[i].gameObject);
                return;
            }
        }
    }
    */
    public GameObject CombineItems(GameObject useItem, GameObject onItem)
    {
        GameObject newItem = new GameObject();

        switch (useItem.name)
        {
            case "Apple":
                switch (onItem.name)
                {
                    case "Apple":
                        return null;
                }
                break;
        }
        return newItem;
    }
}
