using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcController : MonoBehaviour
{
    public string npcName;
    public GameObject player;
    public GameObject chatBubble;
    public int targetRange;

    private void Start()
    {
     
    }

    public void Talk()
    {
        Debug.Log("Talking");
    }
}
