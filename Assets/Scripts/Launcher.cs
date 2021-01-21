using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class Launcher : MonoBehaviour
{
    public GameObject playButton;
    public GameObject controlPanel;
    public GameObject progressLabel;
    public GameObject mainMenu;
    public Image loadingBar;
    public Text progressText;
    public GameObject SceneObject;
    public GameObject easyModeToggle;
    public GameObject rememberPasswordToggle;
    public GameObject passwordText;

    private PlayerData playerData;


    void Start()
    {
        playerData = gameObject.GetComponent<DataHandler>().playerData;
        if (PlayerPrefs.GetInt("EasyModeEnabled") == 1)
        {
            easyModeToggle.GetComponent<Toggle>().isOn = true;
        }
        if(PlayerPrefs.GetInt("RememberPassword") == 1)
        {
            rememberPasswordToggle.GetComponent<Toggle>().isOn = true;
            passwordText.GetComponent<InputField>().text = PlayerPrefs.GetString("Password");
        }
        else if(PlayerPrefs.GetInt("RememberPassword") != 1)
        {
            rememberPasswordToggle.GetComponent<Toggle>().isOn = false;
            passwordText.GetComponent<InputField>().text = null;
        }

        //   progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void Connect()
    {
        // SceneManager.LoadSceneAsync("Jungle", LoadSceneMode.Single);
      //  dataHandler.SaveData();
        StartCoroutine(LoadScene(playerData.currentSceneId));
   //     progressText.text = "Starting Game";
        //Change the Text to show the Scene is ready
      //  loadingBar.fillAmount = 1f;
      //  SceneManager.LoadScene(playerData.currentSceneId);

    }

    public void EnableEasyMode(bool value)
    {
        int tempNum = 0;
        if (value == true)
        {
            tempNum = 1;
        }
        else
        {
            tempNum = 0;
        }
        PlayerPrefs.SetInt("EasyModeEnabled", tempNum);
    }
    public void EnableRememberPassword(bool value)
    {
        int tempNum = 0;
        if (value == true)
        {
            tempNum = 1;
        }
        else
        {
            tempNum = 0;
        }
        PlayerPrefs.SetInt("RememberPassword", tempNum);
    }

    public IEnumerator LoadScene(int sceneId)
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Single);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
          //  loadingBar.fillAmount += (asyncOperation.progress + .1f) * 100 - 50;

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                progressText.text = "Starting Game";
                //Change the Text to show the Scene is ready
                loadingBar.fillAmount = 1f;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}