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
    }

	void Update ()
	{

    }
}
