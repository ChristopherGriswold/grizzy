using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int slotId;
    public bool isFilled;
    public GameObject item;
    public GameObject slotSelector;
    public RawImage itemImage;
    public GameObject itemDetails;
    public GameObject playerDetails;
    public UseButtonHandler useButtonHandler;
    public DropButtonHandler dropButtonHandler;
    public GameObject slotText;
    public AudioClip selectSlotSound;
    public AudioClip deselectSlotSound;

    public bool isSelected;
    private ItemDetails itemDetailsScript;
    private SlotSelector slotSelectorScript;
    private AudioSource audioSource;
    private PlayerData playerData;

    private void Awake()
    {
        PreLoadSlots();
        audioSource = GetComponent<AudioSource>();
        itemDetailsScript = itemDetails.GetComponent<ItemDetails>();
        slotSelectorScript = slotSelector.GetComponent<SlotSelector>();
        UpdateSlotText();
    }

    public void PreLoadSlots()
    {
        playerData =  gameObject.transform.root.GetComponent<DataHandler>().playerData;
    }

    public void SelectSlot()
    {
        if (!isFilled)
        {
            return;
        }

        if (isSelected)
        {
            DeselectSlot();
            return;
        }
        if (useButtonHandler.isCombining)
        {
            
          //  FillSlot(useButtonHandler.Combine(item));
            useButtonHandler.Combine(item);
            //   SelectSlot();
            DeselectSlot();
            return;
        }
        audioSource.PlayOneShot(selectSlotSound);
        isSelected = true;
        slotSelector.SetActive(true);
        slotSelectorScript.ActivateSlotSelector(this);
        itemDetailsScript.SetItemDetails(item);
        itemDetails.SetActive(true);
        useButtonHandler.EnableButton(item, this);
        dropButtonHandler.EnableDropButton(item, this);
    }

    public void DeselectSlot()
    {
        if (this.isActiveAndEnabled)
        {
          //  audioSource.PlayOneShot(deselectSlotSound);
        }
        isSelected = false;
        itemDetails.SetActive(false);
        slotSelector.SetActive(false);
        useButtonHandler.DisableButton();
        dropButtonHandler.DisableDropButton();
    }

    public void FillSlot(GameObject i)
    {
        PreLoadSlots();
        ItemHandler itemHandler = i.GetComponent<ItemHandler>();
        item = i;
        isFilled = true;
        itemImage.enabled = true;
        itemImage.texture = i.GetComponent<RawImage>().texture;
        UpdateSlotText();
        itemHandler.inSlot = this;
        playerData.items[slotId] = itemHandler.itemName;
        playerData.itemAmounts[slotId] = itemHandler.amount;
    }

    public void EmptySLot()
    {
        item = null;
        isFilled = false;
        itemImage.enabled = false;
        itemImage.texture = null;
        UpdateSlotText();
        playerData.items[slotId] = "Empty";
        playerData.itemAmounts[slotId] = 0;
    }

    public void AddToItem(int a)
    {
        ItemHandler itemHandler = item.GetComponent<ItemHandler>();
        itemHandler.amount += a;
        playerData.itemAmounts[slotId] = itemHandler.amount;
        UpdateSlotText();
    }

    public void SubtractFromItem(int a)
    {
        ItemHandler itemHandler = item.GetComponent<ItemHandler>();
        itemHandler.amount -= a;
        playerData.itemAmounts[slotId] = itemHandler.amount;
        UpdateSlotText();
    }

    public void UpdateSlotText()
    {
        if (!item || (item.GetComponent<ItemHandler>().amount < 2 && !(item.GetComponent<ItemHandler>().type == "Weapon")))
        {
            slotText.GetComponent<Text>().text = "";
        }
        else
        {
            slotText.GetComponent<Text>().text = item.GetComponent<ItemHandler>().amount.ToString();
        }
        if (item)
        {
            playerData.itemAmounts[slotId] = item.GetComponent<ItemHandler>().amount;
        }
    }
}
