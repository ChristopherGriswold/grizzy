using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class EnemyController : MonoBehaviourPunCallbacks
{
    public string enemyName;
    public int level;
    public int startHealth;
    public int health;
    public int targetRange;
    public int maxInstances;
    public int damageToPlayerAmount;
    public GameObject item0;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public AudioClip hurtSound;
    public AudioClip dieSound;
    public AudioClip biteSound;
    public SpawnItems spawnItems;
    public GameObject xpPrefab;
    public int xpAmount;
    private GameObject gameManager;
    public AudioSource audioSource;
    private AgentMove agentMove;
    private GameObject xpPopup;
    private Vector3 killerPosition;

   // Dictionary<string, float> dropTable = new Dictionary<string, float>();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        agentMove = GetComponent<AgentMove>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        health = startHealth;
        gameObject.name = enemyName;
        
    }

    public void PlayBiteSound()
    {
        audioSource.clip = biteSound;
        audioSource.pitch = Random.value + 1;
        audioSource.Play();
    }

    public void Damage(int amount, GameObject killer)
    {
        agentMove.Flee(killer);
        // health -= amount;
        photonView.RPC("RpcDamage", RpcTarget.All, amount);
        if (health <= 0)
        {
            Die(killer);
          //  photonView.RPC("RpcDie", PhotonTargets.All, fromPos);
        }
        else
        {
            audioSource.clip = hurtSound;
            audioSource.pitch = Random.value + 1;
            audioSource.PlayDelayed(.25f);
        }
    }
    [PunRPC]
    public void RpcDamage(int amount)
    {
        health -= amount;
    }

    void Die(GameObject killer)
    {
        killerPosition = killer.transform.position;
        if (xpPopup)
        {
            Destroy(xpPopup);
        }
        xpPopup = Instantiate(xpPrefab, transform.GetChild(0).position, Quaternion.identity);
        xpPopup.GetComponent<XPPopup>().Popup(killer, xpAmount);
      //  killer.GetComponent<XPController>().GainXP("Combat", modifiedXpAmount);
        DropItems();

        photonView.RPC("RpcDie", RpcTarget.All, null);
    }

    [PunRPC]
    public void RpcDie()
    {
        gameObject.SetActive(false);
        Invoke("Reincarnate", 1f);
        //  gameManager.GetComponent<SpawnItems>().SpawnOne(gameObject);
        //  PhotonNetwork.Destroy(gameObject);
    }

    public void Reincarnate()
    {
        health = startHealth;
        gameManager.GetComponent<SpawnItems>().MoveOne(gameObject);
        gameObject.SetActive(true);
    }

    void DropItems()
    {
        Vector3 directionToPlayer = killerPosition - transform.position;
        Physics.IgnoreLayerCollision(2, 2, true);
        Physics.IgnoreLayerCollision(15, 15, true);
        GameObject newItem;
        float randomNum = Random.value;
        if (randomNum > .92f)
        {
            newItem = Instantiate(item2, gameObject.transform.position, Quaternion.identity);
        }else if(randomNum > .65f)
        {
            newItem = Instantiate(item1, gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            newItem = Instantiate(item0, gameObject.transform.position, Quaternion.identity);
        }
    //    newItem.transform.position = gameObject.transform.position;
        newItem.GetComponent<AudioSource>().PlayOneShot(dieSound);
        newItem.GetComponent<Rigidbody>().AddForce((Vector3.up * 30) + directionToPlayer * 2.5f);
       // newItem.GetComponent<Rigidbody>().AddRelativeForce(directionToPlayer * 1);  //Random.Range(0, 0), Random.Range(30, 30), Random.Range(0, 0)
    }

}
