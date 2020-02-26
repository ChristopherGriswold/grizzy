using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class XPController : MonoBehaviour
{
    public GameObject player;
    public GameObject XpOverlay;
    public TextMeshProUGUI title;
    public TextMeshProUGUI level;
    public Image barFillImage;
    public GameObject levelUpText;

    private bool doneCounting;
    private bool isLeveling;
    
    private PlayerVariables playerVariables;

    private void Start()
    {
       playerVariables = player.GetComponent<PlayerVariables>();
    }
    public void GainXp(string xptype, int amount)
    {
        title.text = xptype + ":";
        switch (xptype)
        {
            case "Attack":
                int currentAttackXp = playerVariables.attackXp;
                int totalXp = currentAttackXp + amount;
                playerVariables.SetAttackXp(currentAttackXp + amount);
                StartCoroutine(UpdateXpOverlay("Attack", currentAttackXp, totalXp));
                break;
            case "Crafting":
                playerVariables.SetAttackXp(playerVariables.craftingXp + amount);
                break;
            case "Gathering":
                playerVariables.SetAttackXp(playerVariables.gatheringXp + amount);
                break;
            case "Intelligence":
                playerVariables.SetAttackXp(playerVariables.cookingXp + amount);
                break;
        }
    }
    public IEnumerator UpdateXpOverlay(string type, int startXp, int endXp)
    {

        XpOverlay.SetActive(true);
        doneCounting = false;
        CancelInvoke();
        int countBy = (int)Mathf.Sqrt(endXp - startXp) - 3;
        if (countBy < 1)
        {
            countBy = 1;
        }
        int tempAttackXp = ((int)Mathf.Pow(startXp, .25f));

        level.text = tempAttackXp.ToString();
        while (startXp <= endXp)
        {
            switch (type)
            {
                case "Attack":
                    int combatLvl = ((int)Mathf.Pow(startXp, .25f));
                    if (tempAttackXp != combatLvl)
                    {
                        level.text = combatLvl.ToString();
                        StopCoroutine("LevelUp");
                        StartCoroutine("LevelUp");
                    }
                    barFillImage.fillAmount = (startXp - Mathf.Pow(combatLvl, 4)) / (Mathf.Pow((combatLvl + 1), 4) - Mathf.Pow(combatLvl, 4));
                    break;
                case "Crafting":
                    int craftingLvl = ((int)Mathf.Pow(playerVariables.craftingXp, .25f));
                    level.text = craftingLvl.ToString();
                    barFillImage.fillAmount = (playerVariables.craftingXp - Mathf.Pow(craftingLvl, 4)) / (Mathf.Pow((craftingLvl + 1), 4) - Mathf.Pow(craftingLvl, 4));
                    break;
                case "Gathering":
                    int gatheringLvl = ((int)Mathf.Pow(playerVariables.gatheringXp, .25f));
                    level.text = gatheringLvl.ToString();
                    barFillImage.fillAmount = (playerVariables.gatheringXp - Mathf.Pow(gatheringLvl, 4)) / (Mathf.Pow((gatheringLvl + 1), 4) - Mathf.Pow(gatheringLvl, 4));
                    break;
                case "Intelligence":
                    int intelligenceLvl = ((int)Mathf.Pow(playerVariables.cookingXp, .25f));
                    level.text = intelligenceLvl.ToString();
                    barFillImage.fillAmount = (playerVariables.cookingXp - Mathf.Pow(intelligenceLvl, 4)) / (Mathf.Pow((intelligenceLvl + 1), 4) - Mathf.Pow(intelligenceLvl, 4));
                    break;
            }
            yield return new WaitForSeconds(.1f);
            tempAttackXp = ((int)Mathf.Pow(startXp, .25f));
            int xpDif = endXp - startXp;
            if (startXp == endXp)
            {
                startXp++;
            }
            else if (countBy > xpDif)
              {
                  startXp += xpDif;
              }
              else
              {
                startXp += countBy;
              }
        }
        doneCounting = true;
        if (!isLeveling)
        {
            CancelInvoke();
            Invoke("CloseXpOverlay", 3f);
        }
    }

    public IEnumerator LevelUp()
    {
        isLeveling = true;
        CancelInvoke();
        levelUpText.SetActive(false);
        levelUpText.SetActive(true);
        TextMeshProUGUI levelText = levelUpText.GetComponent<TextMeshProUGUI>();

        levelText.text = "";
        yield return new WaitForSeconds(.1f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSeconds(.1f);
        levelText.text = "";
        yield return new WaitForSeconds(.1f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSeconds(.1f);
        levelText.text = "";
        yield return new WaitForSeconds(.1f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSeconds(.1f);
        levelText.text = "";
        yield return new WaitForSeconds(.1f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSeconds(.1f);
        levelText.text = "";
        yield return new WaitForSeconds(.1f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSeconds(.1f);
        levelText.text = "";

        yield return new WaitForSeconds(.25f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSeconds(.49f);
        levelText.text = "";
        yield return new WaitForSeconds(.49f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSeconds(.49f);
        levelText.text = "";
        yield return new WaitForSeconds(.49f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSeconds(.49f);
        levelText.text = "";
        yield return new WaitForSeconds(.49f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSeconds(1f);

        isLeveling = false;
        if (doneCounting)
        {
            CloseXpOverlay();
        }

    }

    public void CloseXpOverlay()
    {
        XpOverlay.SetActive(false);
        levelUpText.SetActive(false);
    }
}
