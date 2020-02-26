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

    private void Start()
    {
        ChangeQualitySetting(2);
    }

    public void CheckSettings()
    {
        lowButton.GetComponent<Image>().color = Color.white;
        highButton.GetComponent<Image>().color = Color.white;
        ultraButton.GetComponent<Image>().color = Color.white;

        currentQualityLevel = QualitySettings.GetQualityLevel();
        switch (currentQualityLevel)
        {
            case 0:
                {
                    lowButton.GetComponent<Image>().color = Color.green;
                    break;
                }
            case 2:
                {
                    highButton.GetComponent<Image>().color = Color.green;
                    break;
                }
            case 5:
                {
                    ultraButton.GetComponent<Image>().color = Color.green;
                    break;
                }
        }
    }
    public void ChangeQualitySetting(int i)
    {
        QualitySettings.SetQualityLevel(i, true);
        CheckSettings();
    }
}
