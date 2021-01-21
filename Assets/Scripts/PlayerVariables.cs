using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject backpackObject;
    public GameObject itemMagnetTrigger;

    private Backpack backpack;
    private PlayerData playerData;


    private void Start()
    {
        backpack = backpackObject.GetComponent<Backpack>();
        playerData = gameObject.GetComponent<DataHandler>().playerData;
        if (playerData.rewardsPurchased.Contains(4))
        {
            itemMagnetTrigger.SetActive(true);
        }

        playerName = playerData.playerName;
        health = playerData.health;

        totalXp = playerData.attackXp + playerData.defenseXp + playerData.huntingXp + playerData.gatheringXp + playerData.craftingXp + playerData.cookingXp;
        attackXp = playerData.attackXp;
        defenseXp = playerData.defenseXp;
        huntingXp = playerData.huntingXp;
        gatheringXp = playerData.gatheringXp;
        craftingXp = playerData.craftingXp;
        cookingXp = playerData.cookingXp;

        cash = playerData.cash;

    }


    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }
    public void SetCash(int c)
    {
        cash = c;
        playerData.cash = cash;
    }
    public void AddCash(int c)
    {
        cash += c;
        SetCash(cash);
    }
    public void RemoveCash(int c)
    {
        cash -= c;
        SetCash(cash);
    }
    public void SetHealth(int h)
    {
        health = h;
        playerData.health = health;
    }
    public void SetTotalXp()
    {
        totalXp = attackXp + defenseXp + huntingXp + gatheringXp + craftingXp + cookingXp;
    }
    public void SetAttackXp(int xp)
    {
        attackXp = xp;
        SetTotalXp();
        playerData.attackXp = attackXp;
    }
    public void SetDefenseXp(int xp)
    {
        defenseXp = xp;
        SetTotalXp();
        playerData.defenseXp = defenseXp;
    }
    public void SetHuntingXp(int xp)
    {
        huntingXp = xp;
        SetTotalXp();
        playerData.huntingXp = huntingXp;
    }
    public void SetGatheringXp(int xp)
    {
        gatheringXp = xp;
        SetTotalXp();
        playerData.gatheringXp = gatheringXp;
    }
    public void SetCraftingXp(int xp)
    {
        craftingXp = xp;
        SetTotalXp();
        playerData.craftingXp = craftingXp;
    }
    public void SetCookingXp(int xp)
    {
        cookingXp = xp;
        SetTotalXp();
        playerData.cookingXp = cookingXp;
    }

    

    public void SetEquipment()
    {
    //    if (PlayerPrefs.HasKey("Held Item"))
     //   {
    //        this.gameObject.GetComponent<EquipmentHandler>().Equip();
     //       PhotonNetwork.Instantiate(PlayerPrefs.GetString("Held Item"), Vector3.zero, Quaternion.identity, 0, null).GetComponent<ItemHandler>().PickUpItem(this.gameObject);
     //   }
    }

}
