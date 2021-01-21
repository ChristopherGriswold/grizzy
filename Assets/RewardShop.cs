using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.CodeDom;

public class RewardShop : MonoBehaviour
{
    public GameObject confirmPuchaseOverlay;
    public GameObject insufficentFunds;
    Reward[] rewards = new Reward[12];
    private GameObject player;
    private Reward reward;

    private DataHandler dataHandler;
    private TextMeshProUGUI playerCash;


    private void Awake()
    {
        player = gameObject.transform.root.gameObject;
        dataHandler = player.GetComponent<DataHandler>();

        rewards = GetComponentsInChildren<Reward>();

    }
    private void OnEnable()
    {
        playerCash = transform.Find("PlayerCash").GetComponent<TextMeshProUGUI>();
        playerCash.text = "$ " + dataHandler.playerData.cash.ToString();
    }

    private void Start()
    {
        for (int i = 0; i < dataHandler.playerData.rewardsPurchased.Count; i++)
        {
            for(int j = 0; j < rewards.Length; j++)
            {
                if (rewards[j].rewardId == dataHandler.playerData.rewardsPurchased[i])
                {
                    rewards[j].Purchased();

                }
            }
        }
    }

    public void ConfirmPurchase()
    {
        switch (reward.rewardId)
        {
            case 1:
                dataHandler.playerData.cash -= reward.cost;
                dataHandler.playerData.rewardsPurchased.Add(reward.rewardId);
                player.GetComponentInChildren<MiniMapController>().EnableEnemyBlips();
                dataHandler.SaveData();
                confirmPuchaseOverlay.SetActive(false);
                gameObject.GetComponent<AudioSource>().Play();
                reward.Purchased();
                playerCash.text = "$ " + dataHandler.playerData.cash.ToString();
                break;
            case 2:
                dataHandler.playerData.cash -= reward.cost;
                dataHandler.playerData.rewardsPurchased.Add(reward.rewardId);     
                dataHandler.SaveData();
                confirmPuchaseOverlay.SetActive(false);
                gameObject.GetComponent<AudioSource>().Play();
                reward.Purchased();
                playerCash.text = "$ " + dataHandler.playerData.cash.ToString();
                break;
            case 3:
                dataHandler.playerData.cash -= reward.cost;
                dataHandler.playerData.rewardsPurchased.Add(reward.rewardId);
                dataHandler.SaveData();
                confirmPuchaseOverlay.SetActive(false);
                gameObject.GetComponent<AudioSource>().Play();
                reward.Purchased();
                playerCash.text = "$ " + dataHandler.playerData.cash.ToString();
                break;
            case 4:
                dataHandler.playerData.cash -= reward.cost;
                dataHandler.playerData.rewardsPurchased.Add(reward.rewardId);
                dataHandler.SaveData();
                confirmPuchaseOverlay.SetActive(false);
                gameObject.GetComponent<AudioSource>().Play();
                reward.Purchased();
                playerCash.text = "$ " + dataHandler.playerData.cash.ToString();
                player.GetComponent<PlayerVariables>().itemMagnetTrigger.SetActive(true);
                break;
            case 5:
                dataHandler.playerData.cash -= reward.cost;
                dataHandler.playerData.rewardsPurchased.Add(reward.rewardId);
                dataHandler.SaveData();
                confirmPuchaseOverlay.SetActive(false);
                gameObject.GetComponent<AudioSource>().Play();
                reward.Purchased();
                playerCash.text = "$ " + dataHandler.playerData.cash.ToString();
                player.GetComponentInChildren<TargetHandler>(true).EnableAutoAim();
                try
                {
                    player.GetComponentInChildren<FireWeapon>(true).EnableAutoAim();
                }
                catch (System.Exception)
                {

                }
                break;
            case 6:
                Debug.Log("EnableReward(): " + reward.rewardId);
                break;
            case 7:
                Debug.Log("EnableReward(): " + reward.rewardId);
                break;
            case 8:
                Debug.Log("EnableReward(): " + reward.rewardId);
                break;
            case 9:
                Debug.Log("EnableReward(): " + reward.rewardId);
                break;
            case 10:
                Debug.Log("EnableReward(): " + reward.rewardId);
                break;
            case 11:
                Debug.Log("EnableReward(): " + reward.rewardId);
                break;
            case 12:
                Debug.Log("EnableReward(): " + reward.rewardId);
                break;

            default:
                break;
        }
    }

    private IEnumerator FlashInsufficentFunds()
    {
        insufficentFunds.SetActive(true);
        yield return new WaitForSecondsRealtime(5f);
        insufficentFunds.SetActive(false);
    }

    public void TryPurchaseReward(int id)
    {
        for(int i = 0; i < rewards.Length; i++)
        {
            if(rewards[i].rewardId == id)
            {
                reward = rewards[i];
                break ;
            }
        }
        if(player.GetComponent<DataHandler>().playerData.cash < reward.cost)
        {
            StartCoroutine(FlashInsufficentFunds());
            return;
        }
        else
        {
            confirmPuchaseOverlay.SetActive(true);
        }
    }

}
