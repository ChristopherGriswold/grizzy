using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private PlayerData playerData;
    private bool isDead = false;

    private void Awake()
    {
        playerData = gameObject.transform.root.GetComponent<PlayerData>();
        health = playerData.health;
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
        playerData.health = health;
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
        LoseHealth(100);
        gameOverScreen.SetActive(true);
        bloodScreenOverlay.SetActive(true);
        for (int i = 0; i < playerData.items.Length; i++)
        {
            playerData.items[i] = "Empty";
            playerData.itemAmounts[i] = 0;
        }
        playerData.questsCompleted = null;
        player.GetComponent<PlayerController>().StopCoroutine("UpdatePlayerPosition");
        playerData.playerPosition = GameObject.Find("PlayerSpawn").transform.position;
        playerData.health = 50;
        player.GetComponent<DataHandler>().SaveData();
        Time.timeScale = 0.15f;
        Time.fixedDeltaTime = 0.02f * 0.15f;
        player.GetComponent<DamagePlayer>().StopCoroutines();
        
      //  GameObject.Find("GameManager").GetComponent<DataHandler>().SaveData();
     //   playerData.GetComponent<DataHandler>().SaveData();
        RawImage bloodScreenImage = bloodScreenOverlay.GetComponent<RawImage>();
        Color currColor = bloodScreenImage.color;
        currColor.a = 1f;
        bloodScreenImage.color = currColor;
        //    healthDisplayText.text = health.ToString();
        //    menuHealth.GetComponent<MenuHealth>().UpdateMenuHealth(health);
        //  playerVariables.SetHealth(health);
        if (player.GetComponentInChildren<FireWeapon>())
        {
            player.GetComponentInChildren<FireWeapon>().canShoot = false;
        }
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        isDead = false;

        gameOverScreen.SetActive(false);
        bloodScreenOverlay.SetActive(false);
        SceneManager.LoadSceneAsync("Jungle");
     //   player.transform.position = GameObject.Find("PlayerSpawn").transform.position;
    //    player.GetComponent<PlayerController>().StartCoroutine("UpdatePlayerPositionData");
    }
}
