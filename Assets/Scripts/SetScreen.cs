using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScreen : MonoBehaviour
{
	void Start ()
	{
        Screen.SetResolution(1280, 720, true);
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel"), true);
        }
        else
        {
            QualitySettings.SetQualityLevel(2, true);
        }
    }

	void Update ()
	{

    }
}
