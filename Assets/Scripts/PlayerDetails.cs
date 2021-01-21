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
    public TextMeshProUGUI attackText;
    public Image attackBarFill;
    public TextMeshProUGUI defenseText;
    public Image defenseBarFill;
    public TextMeshProUGUI huntingText;
    public Image huntingBarFill;
    public TextMeshProUGUI gatheringText;
    public Image gatheringBarFill;
    public TextMeshProUGUI craftingText;
    public Image craftingBarFill;
    public TextMeshProUGUI cookingText;
    public Image cookingBarFill;

    public TextMeshProUGUI cashText;


    int totalLvl;
    int attackLvl;
    int defenseLvl;
    int huntingLvl;
    int gatheringLvl;
    int craftingLvl;
    int cookingLvl;

    private PlayerVariables playerVariables;
    private DataHandler dataHandler;
	void Awake ()
	{
        playerVariables = player.GetComponent<PlayerVariables>();
        dataHandler = gameObject.transform.root.GetComponent<DataHandler>();
    }

    public void UpdateLevels()
    {
        attackLvl = ((int)Mathf.Pow(playerVariables.attackXp, .25f));
        defenseLvl = ((int)Mathf.Pow(playerVariables.defenseXp, .25f));

        huntingLvl = ((int)Mathf.Pow(playerVariables.huntingXp, .25f));
        gatheringLvl = ((int)Mathf.Pow(playerVariables.gatheringXp, .25f));

        craftingLvl = ((int)Mathf.Pow(playerVariables.craftingXp, .25f));
        cookingLvl = ((int)Mathf.Pow(playerVariables.cookingXp, .25f));

        totalLvl = ((int)Mathf.Pow(playerVariables.totalXp, .25f));
    }

    public int GetAttackLvl()
    {
        return attackLvl;
    }

    private void OnEnable()
    {

        UpdateLevels();

        nameText.text = playerVariables.playerName;
        totalText.text = totalLvl.ToString();
        totalBarFill.fillAmount = (playerVariables.totalXp - Mathf.Pow(totalLvl, 4)) / (Mathf.Pow((totalLvl + 1), 4) - Mathf.Pow(totalLvl, 4));

        attackText.text = attackLvl.ToString();
        attackBarFill.fillAmount = (playerVariables.attackXp - Mathf.Pow(attackLvl, 4)) / (Mathf.Pow((attackLvl + 1), 4) - Mathf.Pow(attackLvl, 4));

        defenseText.text = defenseLvl.ToString();
        defenseBarFill.fillAmount = (playerVariables.defenseXp - Mathf.Pow(defenseLvl, 4)) / (Mathf.Pow((defenseLvl + 1), 4) - Mathf.Pow(defenseLvl, 4));

        huntingText.text = huntingLvl.ToString();
        huntingBarFill.fillAmount = (playerVariables.huntingXp - Mathf.Pow(huntingLvl, 4)) / (Mathf.Pow((huntingLvl + 1), 4) - Mathf.Pow(huntingLvl, 4));

        gatheringText.text = gatheringLvl.ToString();
        gatheringBarFill.fillAmount = (playerVariables.gatheringXp - Mathf.Pow(gatheringLvl, 4)) / (Mathf.Pow((gatheringLvl + 1), 4) - Mathf.Pow(gatheringLvl, 4));

        craftingText.text = craftingLvl.ToString();
        craftingBarFill.fillAmount = (playerVariables.craftingXp - Mathf.Pow(craftingLvl, 4)) / (Mathf.Pow((craftingLvl + 1), 4) - Mathf.Pow(craftingLvl, 4));

        cookingText.text = cookingLvl.ToString();
        cookingBarFill.fillAmount = (playerVariables.cookingXp - Mathf.Pow(cookingLvl, 4)) / (Mathf.Pow((cookingLvl + 1), 4) - Mathf.Pow(cookingLvl, 4));

        cashText.text = "$" + dataHandler.playerData.cash.ToString();
    }
}
