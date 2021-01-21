using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeQuality : MonoBehaviour
{
    public GameObject lowButton;
    public GameObject highButton;
    public GameObject ultraButton;
    public GameObject startCamera;

    public int currentQualityLevel;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel"));
        }
    }

    private void OnEnable()
    {
        UpdateSettings(QualitySettings.GetQualityLevel());
    }

    public void UpdateSettings(int lvl)
    {

        switch (lvl)
        {
            case 0:
                {
                    lowButton.GetComponent<Image>().color = Color.green;
                    highButton.GetComponent<Image>().color = Color.white;
                    ultraButton.GetComponent<Image>().color = Color.white;
                    break;
                }
            case 2:
                {
                    highButton.GetComponent<Image>().color = Color.green;
                    lowButton.GetComponent<Image>().color = Color.white;
                    ultraButton.GetComponent<Image>().color = Color.white;
                    break;
                }
            case 5:
                {
                    ultraButton.GetComponent<Image>().color = Color.green;
                    highButton.GetComponent<Image>().color = Color.white;
                    lowButton.GetComponent<Image>().color = Color.white;
                    break;
                }
        }
    }


    public void ChangeQualitySetting(int i)
    {
        PlayerPrefs.SetInt("QualityLevel", i);
        QualitySettings.SetQualityLevel(i, true);
        UpdateSettings(i);
    }
}
