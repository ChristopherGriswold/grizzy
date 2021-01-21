using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public AudioClip combineSound;
    public GameObject cannotEquipText;
    public bool isCombining;

    private bool buttonIsEnabled;
    private HealthController healthController;
    public GameObject selectedItem;
    public ItemHandler selectedItemHandler;
    private Slot selectedSlot;
    private AudioSource audioSource;
    private AudioClip useItemAudioClip;
    private GameObject tempObject;
    private XPController xpController;

    private void Start()
    {
        healthController = Player.GetComponentInChildren<HealthController>();
        xpController = Player.GetComponent<XPController>();
      //  weaponButtonHandler = weaponButton.GetComponent<WeaponButtonHandler>();
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
        yield return new WaitForSecondsRealtime(3f);
        cannotEquipText.SetActive(false);
    }

    private void OnDisable()
    {
        cannotEquipText.SetActive(false);
    }

    public void Equip(GameObject i)
    {
        if (i.GetComponent<ItemHandler>().type == "Weapon")
        {
            int combatLvl = ((int)Mathf.Pow(Player.GetComponent<PlayerVariables>().attackXp, .25f));
            if ((combatLvl >= selectedItemHandler.levelRequired))
            {
                if (weaponSlot.GetComponent<Slot>().isFilled)
                {
                    SwapEquipment(i);
                    return;
                }
                weaponHud.SetActive(true);
                i.GetComponent<FireWeapon>().enabled = true;
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
        audioSource = GetComponent<AudioSource>();
        useItemAudioClip = i.GetComponent<ItemHandler>().useItemSound;
        audioSource.PlayOneShot(useItemAudioClip);
        DisableButton();
        i.GetComponent<EquipmentHandler>().Equip();
        if (selectedSlot)
        {
            selectedSlot.EmptySLot();
            selectedSlot.DeselectSlot();
        }
        selectedItemHandler.isEquipped = true;
    }

    public void Unequip(GameObject i)
    {
        if (backpack.GetComponent<Backpack>().AddToBackpack(i))
        {
            if (i.GetComponent<ItemHandler>().type == "Weapon")
            {
                weaponHud.SetActive(false);
                weaponHud.GetComponent<RawImage>().texture = null;
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
            weaponSlotSlot.item.GetComponent<ItemHandler>().isEquipped = false;
            weaponSlotSlot.item.GetComponent<FireWeapon>().enabled = false;
            weaponSlotSlot.item.SetActive(false);
        }

        useItemAudioClip = selectedItemHandler.useItemSound;
        audioSource.PlayOneShot(useItemAudioClip);
        DisableButton();
        tempObject.SetActive(true);
        tempObject.GetComponent<EquipmentHandler>().Equip();
        weaponSlot.GetComponent<Slot>().FillSlot(tempObject);
        weaponHud.GetComponent<RawImage>().texture = tempObject.GetComponent<RawImage>().texture;
        selectedItemHandler.isEquipped = true;
        selectedSlot.DeselectSlot();
    }
    public void SwapEquipment(GameObject i)
    {
        
        
        tempObject = i;
        if (selectedSlot)
        {
            selectedSlot.EmptySLot();
        }
        if (i.GetComponent<ItemHandler>().type == "Weapon")
        {
            Slot weaponSlotSlot = weaponSlot.GetComponent<Slot>();
            if (backpack.GetComponent<Backpack>().AddToBackpack(weaponSlot.GetComponent<Slot>().item))
            {
                weaponSlotSlot.item.GetComponent<EquipmentHandler>().Unequip();
                weaponSlotSlot.item.GetComponent<ItemHandler>().isEquipped = false;
                weaponSlotSlot.item.GetComponent<FireWeapon>().enabled = false;
                weaponSlotSlot.item.SetActive(false);
            }
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
        if (selectedSlot)
        {
            selectedSlot.DeselectSlot();
        }
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
                    resultItem = (GameObject)Instantiate(Resources.Load(selectedItemHandler.combinationsList[i].resultObject.name), go.transform.parent.transform.position, go.transform.parent.transform.rotation);
                    int resultAmount;
                    if (selectedItemHandler.amount > goItemHandler.amount)
                    {

                        resultAmount = goItemHandler.amount;
                        selectedSlot.SubtractFromItem(goItemHandler.amount);
                        resultItem.GetComponent<ItemHandler>().amount = resultAmount;
                        goItemHandler.inSlot.EmptySLot();

                    }

                    else if(selectedItemHandler.amount < goItemHandler.amount)
                    {
                        resultAmount = selectedItemHandler.amount;
                        goItemHandler.inSlot.SubtractFromItem(selectedItemHandler.amount);
                        resultItem.GetComponent<ItemHandler>().amount = resultAmount;
                        selectedSlot.EmptySLot();

                    }
                    else if (selectedItemHandler.amount == goItemHandler.amount)
                    {
                        resultAmount = selectedItemHandler.amount;
                        resultItem.GetComponent<ItemHandler>().amount = resultAmount;
                        goItemHandler.inSlot.EmptySLot();
                        selectedSlot.EmptySLot();

                    }

                    selectedSlot.DeselectSlot();
                    
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