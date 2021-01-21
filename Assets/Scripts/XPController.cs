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
    public GameObject xpPopup;
    public AudioClip echoClip;

    private bool doneCounting;
    private bool isLeveling;
    private TextMeshProUGUI xpPopupText;
    private RectTransform xpPopupRectTransform;
    private Vector3 xpPopupOriginalPos;
    private AudioSource audioSource;
    private int xpAmount;

    public int currentShownXp;
    public int currentXpTotal;
    
    private PlayerVariables playerVariables;

    private void Start()
    {
        playerVariables = player.GetComponent<PlayerVariables>();
        xpPopupText = xpPopup.GetComponent<TextMeshProUGUI>();
        xpPopupRectTransform = xpPopup.GetComponent<RectTransform>();
        xpPopupOriginalPos = xpPopupRectTransform.localPosition;
        audioSource = xpPopup.GetComponent<AudioSource>();
    }

    public void GainXp(string xptype, int amount)
    {
        xpAmount += amount;
    //    StopCoroutine("SlideXpPopup");
    //    xpPopup.SetActive(false);
        xpPopupRectTransform.localPosition = xpPopupOriginalPos;
      //  xpPopupText.text = "+" + amount + "XP";
        xpPopup.SetActive(true);
        StartCoroutine("SlideXpPopup");
        StartCoroutine("RackupPoints");
        title.text = xptype + ":";
        switch (xptype)
        {
            case "Attack":
                int currentAttackXp = playerVariables.attackXp;
                int newAttackXp = currentAttackXp + amount;
                playerVariables.SetAttackXp(currentAttackXp + amount);
                StartCoroutine(UpdateXpOverlay("Attack", currentAttackXp, newAttackXp));
                break;
            case "Crafting":
                int currentCraftingXp = playerVariables.craftingXp;
                int newCraftingXp = currentCraftingXp + amount;
                playerVariables.SetCraftingXp(currentCraftingXp + amount);
                StartCoroutine(UpdateXpOverlay("Crafting", currentCraftingXp, newCraftingXp));
                break;
            case "Cooking":
                int currentCookingXp = playerVariables.cookingXp;
                int newCookingXp = currentCookingXp + amount;
                playerVariables.SetCookingXp(currentCookingXp + amount);
                StartCoroutine(UpdateXpOverlay("Cooking", currentCookingXp, newCookingXp));
                break;
            case "Gathering":
                int currentGatheringXp = playerVariables.gatheringXp;
                int newGatheringXp = currentGatheringXp + amount;
                playerVariables.SetGatheringXp(currentGatheringXp + amount);
                StartCoroutine(UpdateXpOverlay("Gathering", currentGatheringXp, newGatheringXp));
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
        int tempXp = ((int)Mathf.Pow(startXp, .25f));

        level.text = tempXp.ToString();
        while (startXp <= endXp)
        {
            switch (type)
            {
                case "Attack":
                    int combatLvl = ((int)Mathf.Pow(startXp, .25f));
                    if (tempXp != combatLvl)
                    {
                        level.text = combatLvl.ToString();
                        StopCoroutine("LevelUp");
                        StartCoroutine("LevelUp");
                    }
                    barFillImage.fillAmount = (startXp - Mathf.Pow(combatLvl, 4)) / (Mathf.Pow((combatLvl + 1), 4) - Mathf.Pow(combatLvl, 4));
                    break;
                case "Crafting":
                    int craftingLvl = ((int)Mathf.Pow(startXp, .25f));
                    if (tempXp != craftingLvl)
                    {
                        level.text = craftingLvl.ToString();
                        StopCoroutine("LevelUp");
                        StartCoroutine("LevelUp");
                    }
                    barFillImage.fillAmount = (startXp - Mathf.Pow(craftingLvl, 4)) / (Mathf.Pow((craftingLvl + 1), 4) - Mathf.Pow(craftingLvl, 4));
                    break;
                case "Cooking":
                    int cookingLvl = ((int)Mathf.Pow(startXp, .25f));
                    if (tempXp != cookingLvl)
                    {
                        level.text = cookingLvl.ToString();
                        StopCoroutine("LevelUp");
                        StartCoroutine("LevelUp");
                    }
                    barFillImage.fillAmount = (startXp - Mathf.Pow(cookingLvl, 4)) / (Mathf.Pow((cookingLvl + 1), 4) - Mathf.Pow(cookingLvl, 4));
                    break;
                case "Gathering":
                    int gatheringLvl = ((int)Mathf.Pow(startXp, .25f));
                    if (tempXp != gatheringLvl)
                    {
                        level.text = gatheringLvl.ToString();
                        StopCoroutine("LevelUp");
                        StartCoroutine("LevelUp");
                    }
                    barFillImage.fillAmount = (startXp - Mathf.Pow(gatheringLvl, 4)) / (Mathf.Pow((gatheringLvl + 1), 4) - Mathf.Pow(gatheringLvl, 4));
                    break;
                case "Intelligence":
                    int intelligenceLvl = ((int)Mathf.Pow(playerVariables.cookingXp, .25f));
                    level.text = intelligenceLvl.ToString();
                    barFillImage.fillAmount = (playerVariables.cookingXp - Mathf.Pow(intelligenceLvl, 4)) / (Mathf.Pow((intelligenceLvl + 1), 4) - Mathf.Pow(intelligenceLvl, 4));
                    break;
            }
            yield return new WaitForSecondsRealtime(.1f);
            tempXp = ((int)Mathf.Pow(startXp, .25f));
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
        yield return new WaitForSecondsRealtime(.1f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSecondsRealtime(.1f);
        levelText.text = "";
        yield return new WaitForSecondsRealtime(.1f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSecondsRealtime(.1f);
        levelText.text = "";
        yield return new WaitForSecondsRealtime(.1f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSecondsRealtime(.1f);
        levelText.text = "";
        yield return new WaitForSecondsRealtime(.1f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSecondsRealtime(.1f);
        levelText.text = "";
        yield return new WaitForSecondsRealtime(.1f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSecondsRealtime(.1f);
        levelText.text = "";

        yield return new WaitForSecondsRealtime(.25f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSecondsRealtime(.49f);
        levelText.text = "";
        yield return new WaitForSecondsRealtime(.49f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSecondsRealtime(.49f);
        levelText.text = "";
        yield return new WaitForSecondsRealtime(.49f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSecondsRealtime(.49f);
        levelText.text = "";
        yield return new WaitForSecondsRealtime(.49f);
        levelText.text = "LEVEL UP!";
        yield return new WaitForSecondsRealtime(1f);

        isLeveling = false;
        if (doneCounting)
        {
            CloseXpOverlay();
        }

    }

    public IEnumerator SlideXpPopup()
    {
        float time = Time.unscaledTime;
        while (Time.unscaledTime - time < 10f)
        {
         //   xpPopup.transform.Translate(Vector3.right * 500/(Mathf.Sqrt(xpAmount)) * Time.unscaledDeltaTime);
            yield return new WaitForSecondsRealtime(.033f);
        }
        xpPopup.SetActive(false);
    }

    public IEnumerator RackupPoints()
    {
        int countBy = (int)Mathf.Sqrt(xpAmount) - 3;
        if (countBy < 1)
        {
            countBy = 1;
        }
        audioSource.Play();
        audioSource.pitch = 1;
        float currentXp = 0;
        while (currentXp < xpAmount && xpAmount != 0)
        {
            xpPopupText.text = "+" + currentXp.ToString() + "XP";
            currentXp += countBy;
            yield return new WaitForSecondsRealtime(.033f);
        }
        if (currentXp >= xpAmount)
        {
            audioSource.Stop();
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(echoClip, 1f);
            xpPopupText.text = "+" + xpAmount.ToString() + "XP";
            xpAmount = 0;
            yield break;
        }
    }

    public void CloseXpOverlay()
    {
        XpOverlay.SetActive(false);
        levelUpText.SetActive(false);
    }
}
