using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeleteThisConsole : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = GameObject.Find("GameManager").GetComponent<DataHandler>().playerData.items[2];
    }
}
