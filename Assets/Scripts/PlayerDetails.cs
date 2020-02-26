using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDetails : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI nameText;
    public Text descriptionText;
    public GameObject statsHeaderObject;
    public TextMeshProUGUI totalText;
    public Image totalBarFill;
    public TextMeshProUGUI combatText;
    public Image combatBarFill;
    public TextMeshProUGUI gatheringText;
    public Image gatheringBarFill;
    public TextMeshProUGUI intelligenceTet;
    public Image intelligenceBarFill;
    public TextMeshProUGUI craftingText;
    public Image craftingBarFill;
    public TextMeshProUGUI cashText;

    int combatLvl;
    int craftingLvl;
    int intelligenceLvl;
    int gatheringLvl;
    int totalLvl;

    private PlayerVariables playerVariables;

	void Awake ()
	{
        playerVariables = player.GetComponent<PlayerVariables>();
    }

    public void UpdateLevels()
    {
        combatLvl = ((int)Mathf.Pow(playerVariables.attackXp, .25f));
        craftingLvl = ((int)Mathf.Pow(playerVariables.craftingXp, .25f));
        intelligenceLvl = ((int)Mathf.Pow(playerVariables.cookingXp, .25f));
        gatheringLvl = ((int)Mathf.Pow(playerVariables.gatheringXp, .25f));
        totalLvl = ((int)Mathf.Pow(playerVariables.totalXp, .25f));
    }

    public int GetCombatLvl()
    {
        return combatLvl;
    }

    private void OnEnable()
    {
        UpdateLevels();

        nameText.text = playerVariables.playerName;
        totalText.text = totalLvl.ToString();
        totalBarFill.fillAmount = (playerVariables.totalXp - Mathf.Pow(totalLvl, 4)) / (Mathf.Pow((totalLvl + 1), 4) - Mathf.Pow(totalLvl, 4));
        combatText.text = combatLvl.ToString();
        combatBarFill.fillAmount = (playerVariables.attackXp - Mathf.Pow(combatLvl, 4)) / (Mathf.Pow((combatLvl + 1), 4) - Mathf.Pow(combatLvl, 4));
        gatheringText.text = gatheringLvl.ToString();
        gatheringBarFill.fillAmount = (playerVariables.gatheringXp - Mathf.Pow(gatheringLvl, 4)) / (Mathf.Pow((gatheringLvl + 1), 4) - Mathf.Pow(gatheringLvl, 4));
        intelligenceTet.text = intelligenceLvl.ToString();
        intelligenceBarFill.fillAmount = (playerVariables.cookingXp - Mathf.Pow(intelligenceLvl, 4)) / (Mathf.Pow((intelligenceLvl + 1), 4) - Mathf.Pow(intelligenceLvl, 4));
        craftingText.text = craftingLvl.ToString();
        craftingBarFill.fillAmount = (playerVariables.craftingXp - Mathf.Pow(craftingLvl, 4)) / (Mathf.Pow((craftingLvl + 1), 4) - Mathf.Pow(craftingLvl, 4));
        cashText.text = "$" + playerVariables.cash.ToString();
    }
}
