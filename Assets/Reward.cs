using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{

    public int rewardId;
    public string rewardName;
    public string rewardDescription;
    public int cost;
    public bool isEnabled;

    public void Purchased()
    {
        TextMeshProUGUI costText = transform.Find("Price").GetComponent<TextMeshProUGUI>();
        costText.text = "Owned";
        costText.color = Color.green;
        GetComponent<Button>().interactable = false;
        isEnabled = true;
    }



}
