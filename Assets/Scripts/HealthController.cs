using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class HealthController : MonoBehaviour {

    public int health;
    public Text healthDisplayText;
    public Texture greenHealth;
    public Texture yellowHealth;
    public Texture orangeHealth;
    public Texture redHealth;
    public GameObject menuHealth;
    public GameObject player;
    public GameObject bloodScreenOverlay;
    public GameObject gameOverScreen;
    public AudioClip dieSound;

    private bool isDead = false;

    private PlayerVariables playerVariables;

    private void Start()
    {
        playerVariables = player.GetComponent<PlayerVariables>();
        health = PlayerPrefs.GetInt("Health");
        UpdateHealth();
    }
    public void UpdateHealth()
    {
        if (health > 100)
        {
            health = 100;
        }
        else if (health <= 0)
        {
            health = 0;
            if (!isDead)
            {
                Die();
            }
        }
        if (health >= 90)
        {
            GetComponent<RawImage>().texture = greenHealth;
        }
        else if (health >= 60)
        {
            GetComponent<RawImage>().texture = yellowHealth;
        }
        else if (health >= 30)
        {
            GetComponent<RawImage>().texture = orangeHealth;
        }
        else
        {
            GetComponent<RawImage>().texture = redHealth;
        }
        healthDisplayText.text = health.ToString();
        menuHealth.GetComponent<MenuHealth>().UpdateMenuHealth(health);
        playerVariables.SetHealth(health);
    }

    public void LoseHealth(int a)
    {
        health -= a;
        UpdateHealth();
    }

    public void ReplenishHealth(int a)
    {
        health += a;
        UpdateHealth();
    }

    public void Die()
    {
        StartCoroutine(DieTimer());
    }

    IEnumerator DieTimer()
    {
        isDead = true;
        string nickname = PlayerPrefs.GetString("PlayerName");
    //    PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("PlayerName", nickname);
        LoseHealth(100);
        Time.timeScale = 0.15f;
        Time.fixedDeltaTime = 0.02f * 0.15f;
        //  ReplenishHealth(50);
        gameOverScreen.SetActive(true);
        bloodScreenOverlay.SetActive(true);
        player.GetComponent<DamagePlayer>().StopCoroutines();
        RawImage bloodScreenImage = bloodScreenOverlay.GetComponent<RawImage>();
        Color currColor = bloodScreenImage.color;
        currColor.a = 1f;
        bloodScreenImage.color = currColor;
        //    healthDisplayText.text = health.ToString();
        //    menuHealth.GetComponent<MenuHealth>().UpdateMenuHealth(health);
        //  playerVariables.SetHealth(health);
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1;
        //   PlayerPrefs.DeleteAll();
        ReplenishHealth(50);
        PlayerPrefs.SetString("PlayerName", nickname);
        isDead = false;
        PhotonNetwork.LoadLevel("Jungle");
    }
}
