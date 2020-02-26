using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class FireWeapon : MonoBehaviourPun
{

    public GameObject ammo;
    public GameObject ammoSpawn;
    public GameObject target;
    public GameObject hitMarkerPrefab;
    public GameObject criticalHitMarkerPrefab;
    public GameObject models;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip dryFireSound;
    public float weaponFireRate;
    public float weaponReloadRate;
    public int magSize;
    public int ammoCount;

    public bool canShoot;

    private Animation anim;
    private Animator anim2;
    private AudioSource audioSource;
    private GameObject player;
    private PlayerVariables playerVariables;
    private GameObject mainCamera;
    private ItemHandler itemHandler;
    private Text weaponHudText;
    private Slot inSlot;
    private Backpack inventory;
    private Queue ammoQueue = new Queue();
    private float countDownTime;
    private bool bowDrawn;
    private int maxBowPower;
   CustomTouchPad _customTouchPad;

    
    private int weaponDamage;
    private int playerAttack;
    private float outcome;
    private float distanceToTarget;
    private float drawStrength;
    private bool easyModeEnabled;


    void Start ()
    {
        if(PlayerPrefs.GetInt("EasyModeEnabled") == 1)
        {
            easyModeEnabled = true;
        }
        LoadArrow();
        anim2 = this.gameObject.GetComponent<Animator>();
    //    anim["Draw"].wrapMode = WrapMode.Once;
     //   anim["Draw"].speed = 1f;
        audioSource = gameObject.GetComponent<AudioSource>();
        player = gameObject.transform.root.gameObject;
        itemHandler = gameObject.GetComponent<ItemHandler>();
        maxBowPower = itemHandler.maxDrawPower;
        weaponDamage = itemHandler.effectInt;
        ammoCount = itemHandler.amount;
        weaponHudText = GameObject.Find("WeaponHUD").GetComponentInChildren<Text>();
        weaponHudText.text = ammoCount.ToString();
        inventory = gameObject.transform.root.GetComponentInChildren<Backpack>(true);
        inSlot = itemHandler.inSlot;
        playerVariables = player.GetComponent<PlayerVariables>();
        mainCamera = Camera.main.gameObject;
    }

    private void LoadArrow()
    {
        GameObject ammoClone = Instantiate(ammo, ammoSpawn.transform);
     //   ammoClone.GetComponent<ArrowFlight>().SetShooter(player);
        ammoQueue.Enqueue(ammoClone);
      //  GameObject ammoClone = Instantiate(ammo, ammoSpawn.transform);
      //  Rigidbody ammoRigidBody = ammoClone.GetComponent<Rigidbody>();
    }

    public void UpdateAmmoCount()
    {
        ammoCount = this.gameObject.GetComponent<ItemHandler>().amount;
        if (weaponHudText)
        {
            weaponHudText.text = ammoCount.ToString();
        }
    }
    public void FireShot()
    {
        StartCoroutine(Shoot(null));
    }
    public void FireShot(GameObject target)
    {
        StartCoroutine(Shoot(target));
    }

    public void InflictDamage(GameObject target)
    {
        if (target && target.activeInHierarchy)
        {
            playerAttack = ((int)Mathf.Pow(playerVariables.attackXp, .25f)); // TEMPORARY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            distanceToTarget = Vector3.Distance(player.transform.position, target.transform.GetChild(0).position);
            if (distanceToTarget > 1000)
            {
                return;
            }
            //  outcome = (playerAttack / (distanceToTarget) / Random.value);
            bool criticalHit = false;
            float criticalModifer = Random.Range(.1f, .2f);
            if (criticalModifer > .17f && drawStrength == maxBowPower)
            {
                criticalHit = true;
                criticalModifer += .2f;
            }
            int damageResult = (int)(((weaponDamage + playerAttack) * (drawStrength/50)) * criticalModifer);
            GameObject hitMarker = Instantiate(hitMarkerPrefab);
            if (criticalHit)
            {
                GameObject criticalHitMarker = Instantiate(criticalHitMarkerPrefab);
                criticalHitMarker.GetComponent<HitMarker>().Popup(player, target.transform.GetChild(0).position, 0);
            }
            target.GetComponent<EnemyController>().Damage(damageResult, player);
            hitMarker.GetComponent<HitMarker>().Popup(player, target.transform.GetChild(0).position, damageResult);
        }
    }

    public void SetPlayerAttack(int atk)
    {
        playerAttack = atk;
    }

    public void Reload()
    {
        GameObject ammoInInventory = inventory.FindInBackpack(this.gameObject);
        if(!ammoInInventory)
        {
            audioSource.PlayOneShot(dryFireSound);
        }
        else
        {
            canShoot = false;
            if(ammoCount + ammoInInventory.GetComponent<ItemHandler>().amount <= magSize)
            {
                ammoCount += ammoInInventory.GetComponent<ItemHandler>().amount;
                inventory.RemoveItem(ammoInInventory);
                Destroy(ammoInInventory);
            }
            else
            {
                ammoInInventory.GetComponent<ItemHandler>().inSlot.SubtractFromItem(magSize - ammoCount);
                ammoCount = magSize;
            }
            audioSource.PlayOneShot(reloadSound);
        }
    }

    public void DrawBow()
    {
        if (canShoot)
        {
            bowDrawn = true;
            audioSource.PlayOneShot(reloadSound);
            //   anim2.SetBool("Draw", true);

            anim2.speed = 1f;
            anim2.Play("Draw");
            StartCoroutine(CountTime());
        }
    }

    public void CancelDrawBow()
    {
        bowDrawn = false;
        audioSource.PlayOneShot(reloadSound);
        anim2.speed = 0.25f;
        anim2.Play("Release");
    }

    IEnumerator CountTime()
    {
        while (bowDrawn)
        {
            countDownTime += Time.deltaTime;
            yield return new WaitForSeconds(.05f);
        }
    }
    IEnumerator Shoot(GameObject target)
    {
        if (canShoot && bowDrawn == true)
        {
            if (ammoCount > 0)
            {
                StopCoroutine(CountTime());
                bowDrawn = false;
                drawStrength = countDownTime * 3000f;
                
                countDownTime = 0;
                if (drawStrength > maxBowPower)
                {
                    drawStrength = maxBowPower;
                }
                anim2.StopPlayback();
                audioSource.PlayOneShot(shootSound);
                canShoot = false;
                //  photonView.RPC("RpcFireWeapon", RpcTarget.All, null);

           //     anim2.SetBool("Draw", false);
             //   anim2.SetBool("Release", true);
                anim2.Play("Release");

                GameObject ammoClone = (GameObject) ammoQueue.Dequeue();
                Rigidbody ammoRigidBody = ammoClone.GetComponent<Rigidbody>();
                ArrowFlight arrowFlight = ammoClone.GetComponent<ArrowFlight>();
                arrowFlight.SetShooter(player);
                arrowFlight.SetFireWeapon(this);

                ammoClone.transform.SetParent(null);

                ammoRigidBody.useGravity = true;
                ammoRigidBody.isKinematic = false;

                ammoRigidBody.AddRelativeForce(0f, 0f, drawStrength);
                if (easyModeEnabled)
                {
                    if(this.gameObject.name == "Legendary Bow")
                    {
                        ammoRigidBody.AddForce(Random.Range(-30f, 30f), Random.Range(-20f, 0f), 0f);
                    }
                    else
                    {
                        ammoRigidBody.AddForce(Random.Range(-30f, 30f), Random.Range(20f, 30f), 0f);
                    }
                    if(target != null)
                    {
                        distanceToTarget = Vector3.Distance(this.gameObject.transform.position, target.transform.position);
                    }
                    ammoRigidBody.AddRelativeForce(0f, 0f, distanceToTarget * Random.Range(10f,20f));
                }
                //  drawStrength = 0;
                arrowFlight.enabled = true;
                ammoCount--;
                itemHandler.SetAmount(ammoCount);
                weaponHudText.text = ammoCount.ToString();
                inSlot.UpdateSlotText();
             //   InflictDamage(target);
                yield return new WaitForSeconds(weaponFireRate);
                canShoot = true;
                LoadArrow();
            }
            else
            {
                Reload();
                weaponHudText.text = ammoCount.ToString();
                itemHandler.SetAmount(ammoCount);
                yield return new WaitForSeconds(weaponReloadRate);
                canShoot = true;
            }
        }
    }   

    public void RegisterTouchPad(CustomTouchPad customTouchPad)
    {
        _customTouchPad = customTouchPad;
    }

    public void ForceAcquireTarget(GameObject target)
    {
        _customTouchPad.AcquireTarget(target);
    }
}
