using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataHandler : MonoBehaviour
{
    public string path;
    public PlayerData playerData;
    private string secretKey = "SecretKey";
    private DateTime dateTime;
    public string jsonData;
    public bool isLoading = false;
    public bool isSaving = false;
    public bool loadFailure;
    public bool fullyLoaded;

    private GameObject player;
    private SQLHandler sqlHandler;

    private void Awake()
    {
        sqlHandler = GetComponent<SQLHandler>();
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerData.playerName = PlayerPrefs.GetString("PlayerName");
        }

        if (this.gameObject.tag == "Player")
        {
            player = this.gameObject;
            path = Application.persistentDataPath + "/data/" + playerData.playerName + "PlayerData.json";
            LoadData();
            fullyLoaded = true;
        //    if (playerData.playerPosition != Vector3.zero)
        //    {
        //        player.transform.position = playerData.playerPosition;
        //    }
        //    player.GetComponentInChildren<Backpack>().SetBackpack();
       //     StartCoroutine("AutoSave");
        }

        
     //   CreateNewSave();
      //  LoadData();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && player)
        {
            SaveData();
            Time.timeScale = 0;
        }
        else if (focus)
        {
            Time.timeScale = 1;
        }
    }

    public void CreateLocalUser(string name)
    {
        playerData.playerName = name;
        path = Application.persistentDataPath + "/data/" + playerData.playerName + "PlayerData.json";
        if (!Directory.Exists(Application.persistentDataPath + "/data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/data");
        }
        if (!File.Exists(Application.persistentDataPath + "/data/" + playerData.playerName + "PlayerData.json"))
        {
            File.Create(path).Close();
            SaveData();
        }
        else
        {
        }
    }


    public void SaveData()
    {
        while(isLoading || isSaving)
        {
        }
        isSaving = true;
        path = Application.persistentDataPath + "/data/" + playerData.playerName + "PlayerData.json";
        try
        {
            playerData.dateTimeModified = DateTime.Now.ToString();
            playerData = InjectFileHash(playerData);
            string contents = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(path, contents);
            StartCoroutine(sqlHandler.PushPlayerDataToServer());
        }
        catch
        {
        }
        isSaving = false;
    }


    public bool LoadData()
    {
        while (isLoading || isSaving)
        {
        }
        isLoading = true;
        path = Application.persistentDataPath + "/data/" + playerData.playerName + "PlayerData.json";
        try
            {
                jsonData = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(jsonData, playerData);
                dateTime = DateTime.Parse(playerData.dateTimeModified);
                string fileHash = playerData.fileHash;
                playerData.fileHash = secretKey;
                string contents = JsonUtility.ToJson(playerData, true);
                string hashContents = Sha1Sum2(contents);
                if (hashContents == fileHash)
                {
                isLoading = false;
                return true;
                }
                else
                {
                isLoading = false;
                return false;
                }
            }
            catch
            {
            isLoading = false;
            return false;
        }
    }
    public PlayerData InjectFileHash(PlayerData pdata)
    {
        pdata.fileHash = secretKey;
        string contents = JsonUtility.ToJson(pdata, true);
        string fileHash = Sha1Sum2(contents);
        pdata.fileHash = fileHash;
        return pdata;
    }

    public string Sha1Sum2(string str)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        byte[] bytes = encoding.GetBytes(str);
        var sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
        return System.BitConverter.ToString(sha.ComputeHash(bytes));
    }
    public IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            SaveData();
        }
    }
}