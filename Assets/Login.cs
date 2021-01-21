using System;
using System.Collections;
using UnityEngine;
using System.Security.Cryptography;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour 
{

    private string username;
    private string passwordHash;
    public string password;
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
    }


    private string url = "http://www.iceybones.com/";
 
    public void SetUsername(string n)
    {
        username = n;
    }

    public void SetPassword(string pw)
    {
        password = pw;
    }
 
    byte[] HashPassword(string password)
    {
        PasswordHash hash = new PasswordHash(password);
        
        byte[] hashBytes = hash.ToArray();
        return hashBytes;
    }

    public static string Sha1Sum2(string str)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        byte[] bytes = encoding.GetBytes(str);
        var sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
        return System.BitConverter.ToString(sha.ComputeHash(bytes));
    }


    public IEnumerator LoadingBar()
    {
        while (loadingBar.fillAmount < 1f)
        {
            loadingBar.fillAmount += .01f;
            yield return new WaitForEndOfFrame();
        }

      /*  while (loadingBar.fillAmount >= .5f)
        {
            loadingBar.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            loadingBar.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }*/

    }

    public void Authenticate()
    {
        loginButton.SetActive(false);
        StartCoroutine("LoadingBar");
        
        StartCoroutine(TryAuthenticate());
    }
    public IEnumerator TryAuthenticate()
    {
        bool isAuthenticated = false;
        progressText.text = "Authenticating";

        string customUrl = url + "Login.aspx";

        WWWForm form = new WWWForm();


        passwordHash = Sha1Sum2(password);

        form.AddField("username", username);
        form.AddField("password", passwordHash);

        using (UnityWebRequest www = UnityWebRequest.Post(customUrl, form))
        {
            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                StopCoroutine(LoadingBar());
                loadingBar.fillAmount = 0f;
                progressText.text = www.error;
                loginButton.SetActive(true);
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
                 //   StopCoroutine(LoadingBar());
                 if(loadingBar.fillAmount < .25f)
                    {
                        loadingBar.fillAmount = 0.25f;
                    }
                    progressText.text = "Authentication Successful";
                    playerData.playerName = username;
                    yield return new WaitForSeconds(.1f);
                    progressText.text = "Loading Player Data";
                    dataHandler.CreateLocalUser(username);
                    if (!dataHandler.LoadData())
                    {
                        progressText.text = "Failed To Load Player Data";
                        StopCoroutine("LoadingBar");
                        loadingBar.fillAmount = 0f;
                        yield return new WaitForSeconds(2f);
                        SceneManager.LoadScene(0);
                    }
                    else
                    {
                        isAuthenticated = true;
                    }
                    if(loadingBar.fillAmount < .5f && isAuthenticated)
                    {
                        loadingBar.fillAmount = .5f;
                    }
                    if (PlayerPrefs.GetInt("RememberPassword") == 1)
                    {
                        PlayerPrefs.SetString("Password", password);
                    }
                    else
                    {
                        PlayerPrefs.SetString("Password", "");
                    }

                    //    dataHandler.SaveData();

                    //  dataHandler.SaveData();
                    if (isAuthenticated)
                    {
                        LaunchGame();
                    }
                }
                else
                {
                    StopCoroutine("LoadingBar");
                    loadingBar.fillAmount = 0f;
                    progressLabel.SetActive(true);
                    progressText.text = "Invalid Password";
                    loginButton.SetActive(true);
                }
            }
        }
    }

    public void LaunchGame()
    {
        this.gameObject.GetComponent<Launcher>().Connect();
    }

public sealed class PasswordHash
    {
        const int SaltSize = 16, HashSize = 20, HashIter = 10000;
        readonly byte[] _salt, _hash;
        public PasswordHash(string password)
        {
            new RNGCryptoServiceProvider().GetBytes(_salt = new byte[SaltSize]);
            _hash = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
        }
        public PasswordHash(byte[] hashBytes)
        {
            Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
        }
        public PasswordHash(byte[] salt, byte[] hash)
        {
            Array.Copy(salt, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hash, 0, _hash = new byte[HashSize], 0, HashSize);
        }
        public byte[] ToArray()
        {
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);
            return hashBytes;
        }
        public byte[] Salt { get { return (byte[])_salt.Clone(); } }
        public byte[] Hash { get { return (byte[])_hash.Clone(); } }
        public bool Verify(string password)
        {
            byte[] test = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
            for (int i = 0; i < HashSize; i++)
                if (test[i] != _hash[i])
                    return false;
            return true;
        }
    }

}
