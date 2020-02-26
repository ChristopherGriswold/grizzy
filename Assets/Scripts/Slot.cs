using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        itemDetailsScript = itemDetails.GetComponent<ItemDetails>();
        slotSelectorScript = slotSelector.GetComponent<SlotSelector>();
        UpdateSlotText();
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
            audioSource.PlayOneShot(deselectSlotSound);
        }
        isSelected = false;
        itemDetails.SetActive(false);
        slotSelector.SetActive(false);
        useButtonHandler.DisableButton();
        dropButtonHandler.DisableDropButton();
    }

    public void FillSlot(GameObject i)
    {
        item = i;
        isFilled = true;
        itemImage.enabled = true;
        itemImage.texture = i.GetComponent<RawImage>().texture;
        UpdateSlotText();
        i.GetComponent<ItemHandler>().inSlot = this;
    }

    public void EmptySLot()
    {
        item = null;
        isFilled = false;
        itemImage.enabled = false;
        itemImage.texture = null;
        UpdateSlotText();
    }

    public void AddToItem(int a)
    {
        item.GetComponent<ItemHandler>().amount += a;
        UpdateSlotText();
    }

    public void SubtractFromItem(int a)
    {
        item.GetComponent<ItemHandler>().amount -= a;
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
    }
}
