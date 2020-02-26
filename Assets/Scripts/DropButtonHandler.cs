using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class DropButtonHandler : MonoBehaviour {
    public Text buttonText;
    public string type;
    public GameObject selectedItem;
    public Slot selectedSlot;
    public GameObject weaponEquipSlot;
    public GameObject armorEquipSlot;
    public GameObject weaponHUD;
    public GameObject armorHUD;
    public GameObject confirmDrop;
    public GameObject weaponButton;

    private bool dropButtonEnabled;
    private WeaponButtonHandler weaponButtonHandler;

    private void Start()
    {
        buttonText.text = "";
      //  weaponButtonHandler = weaponButton.GetComponent<WeaponButtonHandler>();
    }

    public void ConfirmedDrop()
    {
        selectedItem.GetComponent<ItemHandler>().DropItem();
        buttonText.text = "";
        selectedSlot.EmptySLot();
        selectedSlot.DeselectSlot();
        if (weaponEquipSlot.GetComponent<Slot>().item == null)
        {
          //  weaponButtonHandler.RegisterEquippedWeapon(null);
            weaponHUD.SetActive(false);
        }
        else if (armorEquipSlot.GetComponent<Slot>().item == null)
        {
            armorHUD.SetActive(false);
        }
    }

   public void Drop()
    {
        if (dropButtonEnabled)
        {
            confirmDrop.SetActive(true);
        }
    }

    public void EnableDropButton(GameObject g, Slot s)
    {
        selectedItem = g;
        selectedSlot = s;
        buttonText.text = "DROP";
        dropButtonEnabled = true;
    }

    public void DisableDropButton()
    {
        buttonText.text = "";
        dropButtonEnabled = false;
    }
}