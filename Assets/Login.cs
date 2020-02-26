using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;

public class Login : MonoBehaviour 
{

    private string username;
    private string passwordHash;
    public string password;
    public GameObject progressLabel;
    public Text progressText;

    
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


    public void Authenticate()
    {
        progressText.text = "Authenticating";
        StartCoroutine(TryAuthenticate());
    }
public IEnumerator TryAuthenticate()
    {
        string customUrl = url + "Login.aspx";

        WWWForm form = new WWWForm();

        form.AddField("username", username);
        form.AddField("password", password); ;

        using (UnityWebRequest www = UnityWebRequest.Post(customUrl, form))
        {
            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
                progressText.text = www.error;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }

                // Print Headers
           //     Debug.Log(sb.ToString());

                // Print Body
          //     Debug.Log(www.downloadHandler.text);

                if(www.downloadHandler.text == "True")
                {
                    progressText.text = "Success";
                    LaunchGame();
                }
                else
                {
                    progressText.text = "Invalid Password";
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
