using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();
}

[System.Serializable]
public class Item
{
    public string name;
    public List<string> ingredients = new List<string>();
}
