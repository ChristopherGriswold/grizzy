using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;

public class PlayerVariables : MonoBehaviour
{
    public string playerName;
    public string chatBubbleText;
    public int health;
    public int maxHealth;
    public int totalXp;
    public int attackXp;
    public int defenseXp;
    public int huntingXp;
    public int gatheringXp;
    public int craftingXp;
    public int cookingXp;
    public int cash;
    public GameObject backpack;


    private void Start()
    {
        //   PlayerPrefs.DeleteAll();
        //  SetBackpack();
        SetPosition();
        if (!PlayerPrefs.HasKey("Health"))
        {
            PlayerPrefs.SetInt("Health", 50);
        }
        if (!PlayerPrefs.HasKey("TotalXp"))
        {
            PlayerPrefs.SetInt("TotalXp", 4);
        }
        if (!PlayerPrefs.HasKey("AttackXp"))
        {
            PlayerPrefs.SetInt("AttackXp", 1);
        }
        if (!PlayerPrefs.HasKey("DefenseXp"))
        {
            PlayerPrefs.SetInt("DefenseXp", Random.Range(1, 1000));
        }
        if (!PlayerPrefs.HasKey("HuntingXp"))
        {
            PlayerPrefs.SetInt("HuntingXp", Random.Range(1, 1000));
        }
        if (!PlayerPrefs.HasKey("GatheringXp"))
        {
            PlayerPrefs.SetInt("GatheringXp", Random.Range(1, 1000));
        }
        if (!PlayerPrefs.HasKey("CraftingXp"))
        {
            PlayerPrefs.SetInt("CraftingXp", Random.Range(1, 1000));
        }
        if (!PlayerPrefs.HasKey("CookingXp"))
        {
            PlayerPrefs.SetInt("CookingXp", Random.Range(1, 1000));
        }

        playerName = PlayerPrefs.GetString("PlayerName");
        health = PlayerPrefs.GetInt("Health");
        totalXp = PlayerPrefs.GetInt("TotalXp");
        attackXp = PlayerPrefs.GetInt("AttackXp");
        defenseXp = PlayerPrefs.GetInt("DefenseXp");
        huntingXp = PlayerPrefs.GetInt("HuntingXp");
        gatheringXp = PlayerPrefs.GetInt("GatheringXp");
        craftingXp = PlayerPrefs.GetInt("CraftingXp");
        cookingXp = PlayerPrefs.GetInt("CookingXp");

        SetTotalXp();
        
    }
    public void GetPosition()
    {
        PlayerPrefs.SetFloat("PlayerPosX", gameObject.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", gameObject.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", gameObject.transform.position.z);
        PlayerPrefs.SetFloat("PlayerRotX", gameObject.transform.rotation.x);
        PlayerPrefs.SetFloat("PlayerRotY", gameObject.transform.rotation.y);
        PlayerPrefs.SetFloat("PlayerRotZ", gameObject.transform.rotation.z);
        PlayerPrefs.SetFloat("PlayerRotW", gameObject.transform.rotation.w);
    }

    public void SetPosition()
    {
        if (!PlayerPrefs.HasKey("PlayerPosX"))
        {
            return;
        }
        Vector3 playerPos;
        Quaternion playerRot;
        playerPos.x = PlayerPrefs.GetFloat("PlayerPosX");
        playerPos.y = PlayerPrefs.GetFloat("PlayerPosY");
        playerPos.z = PlayerPrefs.GetFloat("PlayerPosZ");

        playerRot.x = PlayerPrefs.GetFloat("PlayerRotX");
        playerRot.y = PlayerPrefs.GetFloat("PlayerRotY");
        playerRot.z = PlayerPrefs.GetFloat("PlayerRotZ");
        playerRot.w = PlayerPrefs.GetFloat("PlayerRotW");


        gameObject.transform.SetPositionAndRotation(playerPos, playerRot);
        PlayerPrefs.DeleteKey("PlayerPosX");
        PlayerPrefs.DeleteKey("PlayerPosY");
        PlayerPrefs.DeleteKey("PlayerPosZ");
        PlayerPrefs.DeleteKey("PlayerRotX");
        PlayerPrefs.DeleteKey("PlayerRotY");
        PlayerPrefs.DeleteKey("PlayerRotZ");
        PlayerPrefs.DeleteKey("PlayerRotW");
    }
    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }
    public void SetCash(int c)
    {
        cash = c;
        PlayerPrefs.SetInt("Cash", cash);
    }
    public void AddCash(int c)
    {
        cash += c;
        PlayerPrefs.SetInt("Cash", cash);
    }
    public void RemoveCash(int c)
    {
        cash -= c;
        PlayerPrefs.SetInt("Cash", cash);
    }
    public void SetHealth(int h)
    {
        health = h;
        PlayerPrefs.SetInt("Health", health);
    }
    public void SetTotalXp()
    {
        totalXp = attackXp + defenseXp + huntingXp + gatheringXp + craftingXp + cookingXp;
        PlayerPrefs.SetInt("TotalXp", totalXp);
    }
    public void SetAttackXp(int xp)
    {
        attackXp = xp;
        SetTotalXp();
        PlayerPrefs.SetInt("AttackXp", attackXp);
    }
    public void SetDefenseXp(int xp)
    {
        defenseXp = xp;
        SetTotalXp();
        PlayerPrefs.SetInt("DefenseXp", defenseXp);
    }
    public void SetHuntingXp(int xp)
    {
        huntingXp = xp;
        SetTotalXp();
        PlayerPrefs.SetInt("HuntingXp", huntingXp);
    }
    public void SetGatheringXp(int xp)
    {
        gatheringXp = xp;
        SetTotalXp();
        PlayerPrefs.SetInt("GatheringXp", gatheringXp);
    }
    public void SetCraftingXp(int xp)
    {
        craftingXp = xp;
        SetTotalXp();
        PlayerPrefs.SetInt("CraftingXp", craftingXp);
    }
    public void SetCookingXp(int xp)
    {
        cookingXp = xp;
        SetTotalXp();
        PlayerPrefs.SetInt("CookingXp", cookingXp);
    }
    

    private void OnApplicationPause(bool pause)
    {
        GetPosition();
        GetBackpack();
    }

    

    private void OnApplicationQuit()
    {
        GetPosition();
        GetBackpack();
    }

    public void GetBackpack()
    {
        GameObject[] slots = backpack.GetComponent<Backpack>().slot;
        GameObject weaponSlot = GetComponentInChildren<CustomTouchPad>().weaponSlot;
        
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Slot>().isFilled)
            {
                PlayerPrefs.SetString("Slot" + i, slots[i].GetComponent<Slot>().item.name);
            }
        }
        if (weaponSlot.GetComponent<Slot>().isFilled)
        {
            PlayerPrefs.SetString("Weapon Slot", weaponSlot.GetComponent<Slot>().item.name);
        }
        else
        {
            PlayerPrefs.DeleteKey("Weapon Slot");
        }

    }

    public void SetBackpack()
    {
        if (PlayerPrefs.HasKey("Weapon Slot"))
        {
            GameObject weapon = PhotonNetwork.Instantiate(PlayerPrefs.GetString("Weapon Slot"), Vector3.zero, Quaternion.identity, 0, null);
            weapon.GetComponent<ItemHandler>().PickUpItem(this.gameObject);
            PlayerPrefs.DeleteKey("Weapon Slot");
            //   weapon.GetComponent<EquipmentHandler>().Equip();
        }
        for (int i = 0; i < 15; i++)
        {
            if(PlayerPrefs.HasKey("Slot"+ i))
            {
                PhotonNetwork.Instantiate(PlayerPrefs.GetString("Slot" + i), Vector3.zero, Quaternion.identity, 0, null).GetComponent<ItemHandler>().PickUpItem(this.gameObject);
                PlayerPrefs.DeleteKey("Slot" + i);
            }
        }
    }

    public void SetEquipment()
    {
        if (PlayerPrefs.HasKey("Held Item"))
        {
            this.gameObject.GetComponent<EquipmentHandler>().Equip();
            PhotonNetwork.Instantiate(PlayerPrefs.GetString("Held Item"), Vector3.zero, Quaternion.identity, 0, null).GetComponent<ItemHandler>().PickUpItem(this.gameObject);
        }
    }

}
