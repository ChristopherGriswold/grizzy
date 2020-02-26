using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TargetHandler : MonoBehaviour
{
    public GameObject targetedEnemy;
    public GameObject targetReticle;
    public GameObject easyTargetReticle;
    public GameObject npcReticle;
    public Image healthBar;
    public Image easyHealthBar;
    public GameObject fpsCam;
    public AudioSource aud;
    public TextMeshPro targetNameText;
    public TextMeshPro easyTargetNameText;
    public TextMeshPro npcNameText;
    public TextMeshPro targetLevelText;
    public TextMeshPro easyTargetLevelText;

    public GameObject itemDatabase;


    private float targetStartHealth;
    private float targetHealth;
    private EnemyController enemyController;

    private bool easyModeEnabled;
    private int rangeInt;
    public bool followTarget;
    private float tiltAngle;
    private float healthBarAmount;
    private Color orange;
    private GameObject player;

    void Awake()
    {
        player = fpsCam.transform.root.gameObject;
        if (PlayerPrefs.GetInt("EasyModeEnabled") == 1)
        {
            targetReticle = easyTargetReticle;
            healthBar = easyHealthBar;
            targetNameText = easyTargetNameText;
            targetLevelText = easyTargetLevelText;
            easyModeEnabled = true;
        }
        orange = new Color(1f, .5f, 0f);
        targetNameText.renderer.sortingOrder = 5;
     //   targetLevelText.renderer.sortingOrder = 5;
     
    }
    private void OnEnable()
    {
        if (easyModeEnabled)
        {
            followTarget = true;
        }
    }

    void LateUpdate ()
    {
        if (!targetedEnemy)
        {
            gameObject.SetActive(false);
            return;
        }

        if (targetedEnemy.activeInHierarchy && rangeInt <= 50)
        {
            if (easyModeEnabled && followTarget)
            {
                Vector3 targetPostition = new Vector3(targetedEnemy.transform.GetChild(0).position.x,
                                       targetedEnemy.transform.GetChild(0).position.y,
                                       targetedEnemy.transform.GetChild(0).position.z) ;

            //    tiltAngle += targetedEnemy.transform.position.y;
              //    tiltAngle = Mathf.Clamp(tiltAngle, -80, 80);

                Vector3 playerRotationTarget = new Vector3(targetedEnemy.transform.GetChild(0).position.x,
                                       player.transform.position.y,
                                       targetedEnemy.transform.GetChild(0).position.z);

                player.transform.LookAt(playerRotationTarget);
                itemDatabase.transform.LookAt(targetPostition); 
                //   touchDeltaPosition = eventData.delta;

            //    Vector3 lookDirection = (targetedEnemy.transform.position - itemDatabase.transform.position).normalized;
            //    itemDatabase.transform.Rotate(lookDirection.x, 0, 0);
             //   tiltAngle += lookDirection.x;
              //  tiltAngle = Mathf.Clamp(tiltAngle, -80, 80);
              //  itemDatabase.transform.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
            }
            UpdateTargetHealth();
            if (PlayerPrefs.GetInt("EasyModeEnabled") == 1)
            {
                transform.position = targetedEnemy.transform.GetChild(0).position;
                targetReticle.transform.localPosition = new Vector3(0, 0, Vector3.Distance(transform.position, fpsCam.transform.position) - .3f);
            }
            else
            {
                transform.position = targetedEnemy.transform.GetChild(1).position;
                targetReticle.transform.localPosition = new Vector3(0, (-0.08f * Vector3.Distance(transform.position, targetedEnemy.transform.position)), Vector3.Distance(transform.position, fpsCam.transform.position) - .3f);
            }
                
            transform.LookAt(fpsCam.transform.position);
            
        //    npcReticle.transform.localPosition = new Vector3(0, 0, Vector3.Distance(transform.position, fpsCam.transform.position) - .3f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void AcquireTarget(GameObject target)
    {
        followTarget = true;
        if (target)
        {
            enemyController = target.GetComponent<EnemyController>();
            npcReticle.SetActive(false);
            targetReticle.SetActive(true);
            targetedEnemy = target;
            aud.Play();
            StartCoroutine("RangeTarget");
            targetStartHealth = (float)enemyController.startHealth;
            targetHealth = (float)enemyController.health;
            UpdateTargetHealth();
            targetNameText.text = enemyController.enemyName;
            targetLevelText.text = "lvl: " + enemyController.level.ToString(); 
        }
    }

    public void AcquireTargetOtherPlayer(GameObject target)
    {
        if (target)
        {
            npcReticle.SetActive(false);
            targetReticle.SetActive(true);
            targetedEnemy = target;
            aud.Play();
            StartCoroutine("RangeTarget");
            targetStartHealth = (float)target.GetComponent<PlayerVariables>().maxHealth;
            targetHealth = (float)target.GetComponent<PlayerVariables>().health;
            //   UpdateTargetHealth((int)targetHealth);
            UpdateTargetHealth();
            targetNameText.text = target.GetComponent<PlayerVariables>().playerName;
         //   targetLevelText.text = "lvl " + target.GetComponent<PlayerVariables>().totalLvl.ToString();
        }
    }

    public void AcquireTargetNPC(GameObject target)
    {
        if (target)
        {
            aud.Play();
            npcNameText.text = target.GetComponent<NpcController>().npcName;
        }
    }

    public void UpdateTargetHealth()
    {
        targetHealth = enemyController.health;
        healthBarAmount = (targetHealth / targetStartHealth) *.75f;
        healthBar.fillAmount = healthBarAmount;

        if(healthBarAmount < .1f)
        {
            healthBar.color = Color.red;
        }
        else if(healthBarAmount < .25f)  
        {
            healthBar.color = orange;
        }
        else if(healthBarAmount < .5f)
        {
            healthBar.color = Color.yellow;
        }
        else
        {
            healthBar.color = Color.green;
        }
    }
    IEnumerator RangeTarget()
    {
        while (targetedEnemy)
        {
            rangeInt = (int)Vector3.Distance(targetedEnemy.transform.position, fpsCam.transform.position);
            yield return new WaitForSeconds(1f);
        }
    }
}
