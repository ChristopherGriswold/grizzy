using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuitApplication : MonoBehaviour
{
    // Start is called before the first frame update
    public void QuitGame()
    {
        gameObject.transform.root.GetComponent<DataHandler>().SaveData();
        if (Application.isEditor)
        {
         //   EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}
