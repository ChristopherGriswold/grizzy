using System;
using System.Collections;
using UnityEngine;
using System.Security.Cryptography;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;
// using System.Data.SqlClient;
using UnityEngine.SceneManagement;
using TMPro;

public class SQLHandler : MonoBehaviour
{

    private string username;
    private int playerCash;
    private bool cheated = false;
    public GameObject progressLabel;
    public Text progressText;
    public Image loadingBar;
    public string dateTimeOfSave;
    public GameObject loginButton;

    private DataHandler dataHandler;
    private PlayerData playerData;

    private void Start()
    {
        dataHandler = gameObject.GetComponent<DataHandler>();
        playerData = dataHandler.playerData;
        StartCoroutine(GetLeaderBoardData());
    }


    private string url = "http://www.iceybones.com/";

    public void SetUsername(string n)
    {
        username = n;
    }

    public IEnumerator PushPlayerDataToServer()
    {
        cheated = playerData.cheated;
        username = playerData.playerName;
        playerCash = playerData.cash;

        string customUrl = url + "Update_SQL.aspx";

        WWWForm form = new WWWForm();


        form.AddField("username", username);
        form.AddField("cash", playerCash);
        if (cheated)
        {
            form.AddField("cheated", 1);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(customUrl, form))
        {
            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                progressText.text = www.error;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }
                if (www.downloadHandler.text == "True")
                {
               //     Debug.Log("Upload Successful");
                }
                else
                {
               //     Debug.Log("Upload Failed");
                }
            }
        }
    }

    public IEnumerator GetLeaderBoardData()
    {

        string customUrl = url + "GetLeaderboard.aspx";

        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Post(customUrl, form))
        {
            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                progressText.text = www.error;
            }
            else
            {
           /*     StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                  //  Debug.Log(dict.Key + " : " + dict.Value);
                    //   Debug.Log(FormatLeaderboadString);
                    //   sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }
           */
                GameObject.Find("LeaderText").GetComponent<TextMeshPro>().text = FormatLeaderboadString(www.downloadHandler.text);
            }
        }
    }

    string FormatLeaderboadString(string s)
    {
        s = s.Replace("&", " - ");
        s = s.Replace("*", " - $ ");
        s = s.Replace("^0", "");
        s = s.Replace("^1", " **Cheater**");
        s = s.Replace("#", "\n");
        return s;
    }

}