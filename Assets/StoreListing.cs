using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreListing : MonoBehaviour
{

    public string itemName;
    public int inSlot;
    public int amount;
    public int price;
    public Texture image;

    public TextMeshProUGUI amountText;
    public TextMeshProUGUI priceText;

    private GameObject player;
    private Store store;


    public void Select()
    {
        store = player.GetComponentInChildren<Store>();
        store.selectedListing = this;
        store.confirmSale.SetActive(true);
    }

    public void PostListing()
    {
        amountText.text = "x" + amount.ToString();
        gameObject.GetComponent<RawImage>().texture = image;
        priceText.text = "$" + price.ToString();
    }

    public void SellItem()
    {
        DataHandler datahandler = player.GetComponent<DataHandler>();
        datahandler.playerData.cash += price * amount;
        datahandler.playerData.items[inSlot] = "Empty";
        datahandler.playerData.itemAmounts[inSlot] = 0;
        datahandler.SaveData();
        player.GetComponentInChildren<Backpack>(true).SetBackpack(false);
        Store store = player.GetComponentInChildren<Store>();
        store.GetComponent<AudioSource>().Play();
        store.playerCash.text = "$" + datahandler.playerData.cash.ToString();

        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.transform.root.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
