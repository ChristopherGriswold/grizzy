using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using TMPro;

public class Store : MonoBehaviour
{
    private GameObject player;
    private DataHandler dataHandler;
    private Backpack backpack;
    public GameObject contentPanel;
    public GameObject confirmSale;
    public TextMeshProUGUI playerCash;

    public StoreListing selectedListing;


    public void ConfirmSale()
    {
        selectedListing.SellItem();
        selectedListing = null;
        confirmSale.SetActive(false);
    }

    public void ClearListings()
    {
        StoreListing[] listings = gameObject.GetComponentsInChildren<StoreListing>();
        for(int i = 0; i < listings.Length; i++)
        {
            Destroy(listings[i].gameObject);
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        ClearListings();
        player = gameObject.transform.root.gameObject;
        dataHandler = player.GetComponent<DataHandler>();
        backpack = player.GetComponentInChildren<Backpack>(true);
        playerCash.text = "$" + dataHandler.playerData.cash.ToString();

        for(int i = 2; i < backpack.slots.Length; i++)
        {
            if (backpack.slots[i].GetComponent<Slot>().isFilled)
            {
                GameObject listing = (GameObject)GameObject.Instantiate(Resources.Load("StoreListing"), contentPanel.transform);
                StoreListing storeListing = listing.GetComponent<StoreListing>();
                ItemHandler itemHandler = backpack.slots[i].GetComponent<Slot>().item.gameObject.GetComponent<ItemHandler>();
                storeListing.name = itemHandler.itemName;
                if(itemHandler.type == "Weapon")
                {
                    storeListing.amount = 1;
                }
                else
                {
                    storeListing.amount = itemHandler.amount;
                }
                storeListing.image = itemHandler.gameObject.GetComponent<RawImage>().texture;
                storeListing.price = itemHandler.value;
                storeListing.inSlot = i;
                storeListing.PostListing();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
