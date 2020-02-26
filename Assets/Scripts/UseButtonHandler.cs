﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UseButtonHandler : MonoBehaviour
{
    public Text buttonText;
    public GameObject Player;
    public GameObject backpack;
    public GameObject weaponHud;
    public GameObject armorHud;
    public GameObject weaponSlot;
    public GameObject armorSlot;
    public GameObject weaponButton;
    public AudioClip errorSound;
    public GameObject cannotEquipText;
    public bool isCombining;

    private bool buttonIsEnabled;
    private HealthController healthController;
    public GameObject selectedItem;
    public ItemHandler selectedItemHandler;
    private Slot selectedSlot;
    private WeaponButtonHandler weaponButtonHandler;
    private AudioSource audioSource;
    private AudioClip useItemAudioClip;
    private GameObject tempObject;

    private void Start()
    {
        healthController = Player.GetComponentInChildren<HealthController>();
        weaponButtonHandler = weaponButton.GetComponent<WeaponButtonHandler>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Clicked()
    {
        if (!buttonIsEnabled)
        {
            return;
        }
        else
        {
            switch (selectedItemHandler.type)
            {
                case "Weapon":
                    if (selectedItemHandler.isEquipped)
                    {
                        Unequip(selectedItem);
                    }
                    else
                    {
                        Equip(selectedItem);
                    }
                    break;
                case "Armor":
                    if (selectedItemHandler.isEquipped)
                    {
                        Unequip(selectedItem);
                    }
                    else
                    {
                        Equip(selectedItem);
                    }
                    break;
                case "Consumable":
                    Use(selectedItem);
                    break;
                case "Ammo":
                    Load(selectedItem);
                    break;
                case "Item":
                    Load(selectedItem);
                    break;
            }
        }
    }

    public void EnableButton(GameObject i, Slot s)
    {
        buttonIsEnabled = true;
        selectedSlot = s;
        selectedItem = i;
        selectedItemHandler = i.GetComponent<ItemHandler>();
        if (selectedItemHandler.isEquipped)
        {
            buttonText.text = "UNEQUIP>>";
        }
        else
        {
            buttonText.text = selectedItemHandler.useButtonText;
        }
    }

    public void DisableButton()
    {
        isCombining = false;
        buttonIsEnabled = false;
        buttonText.text = "";
        buttonText.color = Color.white;
    }

    private IEnumerator CantEquipNotification()
    {
        cannotEquipText.SetActive(true);
        yield return new WaitForSeconds(3f);
        cannotEquipText.SetActive(false);
    }

    public void Equip(GameObject i)
    {
        if (i.GetComponent<ItemHandler>().type == "Weapon")
        {
            if((Player.GetComponentInChildren<PlayerDetails>().GetCombatLvl() >= selectedItemHandler.levelRequired))
            {
                if (weaponSlot.GetComponent<Slot>().isFilled)
                {
                    SwapEquipment(i);
                    return;
                }
                weaponHud.SetActive(true);
                weaponButtonHandler.RegisterEquippedWeapon(i);
                weaponSlot.GetComponent<Slot>().FillSlot(i);
                weaponHud.GetComponent<RawImage>().texture = i.GetComponent<RawImage>().texture;
            }
            else
            {
                StartCoroutine(CantEquipNotification());
                return;
            }
        }
        else if(i.GetComponent<ItemHandler>().type == "Armor")
        {
            if (armorSlot.GetComponent<Slot>().isFilled)
            {
                SwapEquipment(i);
                return;
            }
            armorHud.SetActive(true);
            armorSlot.GetComponent<Slot>().FillSlot(i);
            armorHud.GetComponent<RawImage>().texture = i.GetComponent<RawImage>().texture;
        }
        useItemAudioClip = selectedItemHandler.useItemSound;
        audioSource.PlayOneShot(useItemAudioClip);
        DisableButton();
        i.GetComponent<EquipmentHandler>().Equip();
        selectedSlot.EmptySLot();
        selectedItemHandler.isEquipped = true;
        selectedSlot.DeselectSlot();
    }

    public void Unequip(GameObject i)
    {
        if (backpack.GetComponent<Backpack>().AddToBackpack(i))
        {
            if (i.GetComponent<ItemHandler>().type == "Weapon")
            {
                weaponHud.SetActive(false);
                weaponHud.GetComponent<RawImage>().texture = null;
                weaponButtonHandler.RegisterEquippedWeapon(null);
            }
            else if (i.GetComponent<ItemHandler>().type == "Armor")
            {
                armorHud.SetActive(false);
                armorHud.GetComponent<RawImage>().texture = null;
            }
            DisableButton();
            i.GetComponent<EquipmentHandler>().Unequip();
            selectedSlot.EmptySLot();
            selectedSlot.DeselectSlot();
            selectedItemHandler.isEquipped = false;
            i.SetActive(false);
        }
        else
        {
            audioSource.PlayOneShot(errorSound);
        }
    }

    public void SwapEquippedWeapon(GameObject i)
    {

        Slot weaponSlotSlot = weaponSlot.GetComponent<Slot>();
        tempObject = i;
        selectedSlot.EmptySLot();
        if (backpack.GetComponent<Backpack>().AddToBackpack(weaponSlot.GetComponent<Slot>().item))
        {
            weaponSlotSlot.item.GetComponent<EquipmentHandler>().Unequip();
            weaponButtonHandler.RegisterEquippedWeapon(null);
            weaponSlotSlot.item.GetComponent<ItemHandler>().isEquipped = false;
            weaponSlotSlot.item.GetComponent<FireWeapon>().enabled = false;
            weaponSlotSlot.item.SetActive(false);
        }

        useItemAudioClip = selectedItemHandler.useItemSound;
        audioSource.PlayOneShot(useItemAudioClip);
        DisableButton();
        tempObject.SetActive(true);
        tempObject.GetComponent<EquipmentHandler>().Equip();
        weaponButtonHandler.RegisterEquippedWeapon(tempObject);
        weaponSlot.GetComponent<Slot>().FillSlot(tempObject);
        weaponHud.GetComponent<RawImage>().texture = tempObject.GetComponent<RawImage>().texture;
        selectedItemHandler.isEquipped = true;
        selectedSlot.DeselectSlot();
    }
    public void SwapEquipment(GameObject i)
    {
        
        
        tempObject = i;
        selectedSlot.EmptySLot();
        if (i.GetComponent<ItemHandler>().type == "Weapon")
        {
            Slot weaponSlotSlot = weaponSlot.GetComponent<Slot>();
            if (backpack.GetComponent<Backpack>().AddToBackpack(weaponSlot.GetComponent<Slot>().item))
            {
                weaponSlotSlot.item.GetComponent<EquipmentHandler>().Unequip();
                weaponButtonHandler.RegisterEquippedWeapon(null);
                weaponSlotSlot.item.GetComponent<ItemHandler>().isEquipped = false;
                weaponSlotSlot.item.GetComponent<FireWeapon>().enabled = false;
                weaponSlotSlot.item.SetActive(false);
            }
            weaponButtonHandler.RegisterEquippedWeapon(tempObject);
            weaponSlot.GetComponent<Slot>().FillSlot(tempObject);
            weaponHud.GetComponent<RawImage>().texture = tempObject.GetComponent<RawImage>().texture;
        }
        else if (i.GetComponent<ItemHandler>().type == "Armor")
        {
            Slot armorSlotSlot = armorSlot.GetComponent<Slot>();
            if (backpack.GetComponent<Backpack>().AddToBackpack(armorSlot.GetComponent<Slot>().item))
            {
                armorSlotSlot.item.GetComponent<EquipmentHandler>().Unequip();
                armorSlotSlot.item.GetComponent<ItemHandler>().isEquipped = false;
                armorSlotSlot.item.SetActive(false);
            }
            armorSlot.GetComponent<Slot>().FillSlot(tempObject);
            armorHud.GetComponent<RawImage>().texture = tempObject.GetComponent<RawImage>().texture;
        }
        useItemAudioClip = selectedItemHandler.useItemSound;
        audioSource.PlayOneShot(useItemAudioClip);
        DisableButton();
        tempObject.SetActive(true);
        tempObject.GetComponent<EquipmentHandler>().Equip();
        selectedItemHandler.isEquipped = true;
        selectedSlot.DeselectSlot();
    }



    public void Load(GameObject i)
    {
        isCombining = true;
        buttonText.color = Color.green;
    }

    public GameObject Combine(GameObject go)
    {
        ItemHandler goItemHandler = go.GetComponent<ItemHandler>();
        GameObject resultItem = new GameObject();
        for(int i = 0; i < selectedItemHandler.combinationsList.Count; i++)
        {
            if (selectedItemHandler.combinationsList[i].combinesWith == go.name)// == i.name) ATTENTION TO THIS!!!!!
            {
                if (selectedItemHandler.type == "Ammo")
                {
                    int ammoAmount = selectedItemHandler.amount;
                    int ammoInWeapon = goItemHandler.amount;
                    int magSize = go.GetComponent<FireWeapon>().magSize;
                    int spaceInMag = magSize - ammoInWeapon;

                    if (ammoAmount <= spaceInMag)
                    {
                        goItemHandler.SetAmount(ammoInWeapon += ammoAmount);
                        selectedSlot.EmptySLot();
                        backpack.GetComponent<Backpack>().ConsumeItem(selectedItem);
                    }
                    else
                    {
                        goItemHandler.SetAmount(magSize);
                        selectedItemHandler.SetAmount(ammoAmount - spaceInMag);

                    }
                    if (spaceInMag > 0)
                    {
                        useItemAudioClip = selectedItemHandler.useItemSound;
                        audioSource.PlayOneShot(useItemAudioClip);
                    }
                    go.GetComponent<FireWeapon>().UpdateAmmoCount();
                    selectedSlot.DeselectSlot();
                }
                else if (selectedItemHandler.type == "Item")
                {
                    selectedSlot.EmptySLot();
                    selectedSlot.DeselectSlot();
                    backpack.GetComponent<Backpack>().ConsumeItem(selectedItem);
                    //  resultItem = Instantiate(selectedItemHandler.combinationsList[i].resultObject, go.transform.parent);
                    resultItem = PhotonNetwork.InstantiateSceneObject(selectedItemHandler.combinationsList[i].resultObject.name, go.transform.parent.transform.position, go.transform.parent.transform.rotation);
                    go.GetComponent<ItemHandler>().inSlot.GetComponent<Slot>().EmptySLot();
                    backpack.GetComponent<Backpack>().ConsumeItem(go);
                    resultItem.GetComponent<ItemHandler>().CraftItem(this.transform.root.gameObject);
                    return resultItem;
                }
                else
                {
                    selectedSlot.EmptySLot();
                    selectedSlot.DeselectSlot();
                }
            }
        }
        DisableButton();
        selectedSlot.DeselectSlot();
        return resultItem;
    }

    public void Use(GameObject i)
    {
        
        DisableButton();
        useItemAudioClip = selectedItemHandler.useItemSound;
        audioSource.PlayOneShot(useItemAudioClip);
        selectedSlot.EmptySLot();
        selectedSlot.DeselectSlot();
        switch (selectedItemHandler.effect)
        {
            case "Health":
                healthController.ReplenishHealth(selectedItemHandler.effectInt);
                break;
        }
        backpack.GetComponent<Backpack>().ConsumeItem(i);
    }
}   