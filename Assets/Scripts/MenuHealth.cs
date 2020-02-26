using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHealth : MonoBehaviour
{
    public Text healthDisplayText;
    public Texture greenHealth;
    public Texture yellowHealth;
    public Texture orangeHealth;
    public Texture redHealth;

    public void UpdateMenuHealth(int newHealth)
    {
        
        healthDisplayText.text = newHealth.ToString();

        if (newHealth > 100)
        {
            newHealth = 100;
        }
        else if (newHealth < 0)
        {
            newHealth = 0;
        }
        if (newHealth >= 90)
        {
            GetComponent<RawImage>().texture = greenHealth;
        }
        else if (newHealth >= 60)
        {
            GetComponent<RawImage>().texture = yellowHealth;
        }
        else if (newHealth >= 30)
        {
            GetComponent<RawImage>().texture = orangeHealth;
        }
        else
        {
            GetComponent<RawImage>().texture = redHealth;
        }
    }
}
