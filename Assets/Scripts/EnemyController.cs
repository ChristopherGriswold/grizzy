using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
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
    public int xpAmount;
    public Material phaseMaterial;
    private float xpModifer;
    private GameObject gameManager;
    private DataHandler dataHandler;
    public AudioSource audioSource;
    private AgentMove agentMove;
    private GameObject xpPopup;
    private GameObject player;
    private Vector3 killerPosition;
    private Vector3 endDir;


   // Dictionary<string, float> dropTable = new Dictionary<string, float>();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        agentMove = GetComponent<AgentMove>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        dataHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<DataHandler>();

        health = startHealth;
        gameObject.name = enemyName;

        if(this.gameObject.name == "Chicken King")
        {
            int bossKills = dataHandler.playerData.bossKills;
            if (bossKills == 0)
            {
                xpAmount = 2500;
            }
        }
        
    }

    public void PlayBiteSound()
    {
        audioSource.clip = biteSound;
        audioSource.pitch = Random.value + 1;
        audioSource.Play();
    }

    public void Damage(int amount, GameObject killer)
    {
        xpModifer += Vector3.Distance(this.gameObject.transform.position, killer.transform.position) / 5;
        agentMove.Flee(killer);
        // health -= amount;
        RpcDamage(amount);
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
    public void RpcDamage(int amount)
    {
        health -= amount;
    }

    void Die(GameObject killer)
    {
        player = killer;
        killerPosition = killer.transform.position;
        killer.GetComponent<XPController>().GainXp("Attack", xpAmount + (int)xpModifer);
        DropItems();
        RpcDie();
    }

    public void RpcDie()
    {
        PhaseOut();
    //    gameObject.SetActive(false);
      //  Invoke("Reincarnate", 1f);
        //  gameManager.GetComponent<SpawnItems>().SpawnOne(gameObject);
        //  PhotonNetwork.Destroy(gameObject);
    }

    public void Reincarnate()
    {
     //   health = startHealth;
        if(this.gameObject.name == "Chicken King")
        {

            dataHandler.playerData.bossKills++;
            dataHandler.SaveData();
        }
        gameManager.GetComponent<SpawnItems>().SpawnOne(gameObject);
        Destroy(this.gameObject);
     //   gameObject.SetActive(true);
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
        if (player.GetComponent<DataHandler>().playerData.rewardsPurchased.Contains(4))
        {
            newItem.GetComponent<Rigidbody>().AddForce((Vector3.up * 30) + directionToPlayer * 2.5f);
        }
        else
        {
            newItem.GetComponent<Rigidbody>().AddForce((Vector3.up * 30));
        }
    }

    public void PhaseOut()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        audioSource.clip = dieSound;
        audioSource.pitch = 1;
        audioSource.Play(); 
        this.gameObject.GetComponent<Animator>().speed = 0;
        endDir = gameObject.GetComponent<AgentMove>().agent.velocity;
        gameObject.GetComponent<AgentMove>().agent.enabled = false;
        gameObject.GetComponent<AgentMove>().StopAllCoroutines();
        gameObject.GetComponent<AgentMove>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        StartCoroutine(DeathAnimation());
        SkinnedMeshRenderer skinnedMeshRenderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        Material currentMaterial = skinnedMeshRenderer.material;
        Material[] mats = skinnedMeshRenderer.materials;
        mats[0] = phaseMaterial;
        skinnedMeshRenderer.materials = mats;
        Invoke("Reincarnate", 2f);
    }
    IEnumerator DeathAnimation()
    {
        float randomRotScale = Random.Range(-100, 100);
        while (true)
        {
            transform.Translate(endDir * Time.deltaTime, Space.World);
        //    transform.Rotate(Vector3.up * Time.deltaTime * 50, Space.World);
            transform.Rotate(Vector3.right * Time.deltaTime * 20, Space.Self);
            transform.Rotate(Vector3.forward * Time.deltaTime * randomRotScale, Space.Self);
            yield return new WaitForEndOfFrame();
        }
    }

}
