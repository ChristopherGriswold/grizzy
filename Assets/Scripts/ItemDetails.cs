using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetails : MonoBehaviour
{
    public GameObject nameObject;
    public GameObject descriptionObject;
    public GameObject typeObject;
    public GameObject levelObject;
    public GameObject effectObject;
    public GameObject valueObject;

    public void SetItemDetails(GameObject i)
    {
        ItemHandler itemHandler = i.GetComponent<ItemHandler>();
        int effectInt = itemHandler.effectInt;
        int level = itemHandler.levelRequired;
        nameObject.GetComponent<Text>().text = i.name;
        descriptionObject.GetComponent<Text>().text = itemHandler.description;
        typeObject.GetComponent<Text>().text = itemHandler.type;
        levelObject.GetComponent<Text>().text = "Level: " + level.ToString();
        effectObject.GetComponent<Text>().text = itemHandler.effect + ": +" + effectInt.ToString();
        valueObject.GetComponent<Text>().text = "Value: $" + itemHandler.value.ToString();
        if(effectInt > 0)
        {
            effectObject.GetComponent<Text>().color = Color.green;
        }
        else if(effectInt < 0)
        {
            effectObject.GetComponent<Text>().color = Color.red;
        }
        else
        {
            effectObject.GetComponent<Text>().color = Color.white;
        }
    }
}
