using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ItemHandler : MonoBehaviourPun
{
    public string itemName;
    public string description;
    public string type;
    public string useButtonText;
    public string effect;
    public int effectInt;
    public int value;
    public int amount = 1;
    public int levelRequired;
    public int maxDrawPower;
    public bool isStackable;
    public bool isStackableInBank;
    public bool isQuestItem;
    public bool shouldExpire;
    public bool shouldBecomeVisibleToAll;
    public bool isEquipped;
    public Slot inSlot;
    public GameObject triggerCollider;

    //key = name of item this item can be combined with. Value = name of item that is created upon combining said items.
    public List<Combinations> combinationsList = new List<Combinations>();

    public AudioClip useItemSound;


    private void Awake()
    {
        gameObject.name = itemName;
        ComeAlive();
    }

    private void ComeAlive()
    {
        if (shouldBecomeVisibleToAll && !photonView.IsSceneView)
        {
            Invoke("MakeVisibleToAll", 5f);
        }
        if (shouldExpire && gameObject.activeInHierarchy)
        {
            StartCoroutine(Expire(30f));
        }
    }

    // Use this for initialization
    void Start ()
    {

        if (type == "Ammo" && !photonView.isRuntimeInstantiated)
        {
            amount = Random.Range(1, 16);
        }
        if(value == 0)
        {
            gameObject.GetComponent<Light>().color = Color.red;
        }else if (value >= 1000)
        {
            gameObject.GetComponent<Light>().color = Color.magenta;
        }else if (value >= 500)
        {
            gameObject.GetComponent<Light>().color = Color.blue;
        }else if (value >= 100)
        {
            gameObject.GetComponent<Light>().color = Color.yellow;
        }
        else
        {
            gameObject.GetComponent<Light>().color = Color.green;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 13)
        {
         //   PickUpItem(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 13)
        {
            PickUpItem(collision.gameObject);
        }
    }

    public void PickUpItem(GameObject player)
    {
        if (this.gameObject.GetPhotonView().IsSceneView || this.gameObject.GetPhotonView().Owner == player.GetPhotonView().Owner)
        {
            if (player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().RoomInBackpack(this.gameObject))
            {
                triggerCollider.SetActive(false);
                gameObject.SetActive(false);
                StopAllCoroutines();
                CancelInvoke("MakeVisibleToAll");
                //   photonView.RPC("RpcParentItem", RpcTarget.All, player.GetPhotonView().ViewID);
                transform.SetParent(player.GetComponent<PlayerReferences>().itemDatabase.transform);
                player.GetComponent<AudioSource>().Play();
                player.GetComponent<ChatManager>().LocalNotification(gameObject);
                player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().AddToBackpack(gameObject);
            }
            else
            {
                player.GetComponent<ChatManager>().LocalNotification("You can't carry any more items. Your backpack is full!", Color.red, false);
            }
        }
        
    }

    public void ReceiveItem(GameObject player)
    {
        if (this.gameObject.GetPhotonView().IsSceneView || this.gameObject.GetPhotonView().Owner == player.GetPhotonView().Owner)
        {
            if (player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().RoomInBackpack(this.gameObject))
            {
                triggerCollider.SetActive(false);
                gameObject.SetActive(false);
                StopAllCoroutines();
                CancelInvoke("MakeVisibleToAll");
                transform.SetParent(player.GetComponent<PlayerReferences>().itemDatabase.transform);
                player.GetComponent<AudioSource>().Play();
                player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().AddToBackpack(gameObject);
            }
            else
            {
                player.GetComponent<ChatManager>().LocalNotification("You can't carry any more items. Your backpack is full!", Color.red, false);
            }
        }

    }

    public void CraftItem(GameObject player)
    {
        StopAllCoroutines();
        triggerCollider.SetActive(false);
        transform.SetParent(player.GetComponent<PlayerReferences>().itemDatabase.transform);
        player.GetComponent<AudioSource>().Play();
        player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().AddToBackpack(gameObject);
        player.GetComponent<ChatManager>().LocalNotification("Crafted: " + gameObject.name, Color.green, true);
    }



        public void DropItem()
    {
        if (isEquipped)
        {
            this.gameObject.GetComponent<EquipmentHandler>().Unequip();
        }
        
        Vector3 originalPos = this.gameObject.transform.parent.transform.position;
        transform.position = originalPos;
        Physics.IgnoreLayerCollision(13, 20, true);
        gameObject.SetActive(true);
        StartCoroutine(TriggerDisabledTime());
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        photonView.RPC("RpcParentItem", RpcTarget.All, 000);
        GetComponent<Rigidbody>().AddRelativeForce(Random.Range(-5, 5), Random.Range(15, 20), Random.Range(15, 20));
        triggerCollider.SetActive(true);
        ComeAlive();
    }

    IEnumerator TriggerDisabledTime()
    {
        yield return new WaitForSeconds(.5f);
        Physics.IgnoreLayerCollision(13, 20, false);
    }

    public GameObject ReplaceItem(GameObject player)
    {
        if (player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().RoomInBackpack())
        {
            StopAllCoroutines();
        //    if (!photonView.isRuntimeInstantiated)
        //    {
        //        Vector3 itemPosition = gameObject.transform.position;
        //        gameObject.SetActive(false);
        //
        //        GameObject newItem = PhotonNetwork.Instantiate(gameObject.name, itemPosition, Quaternion.identity, 0, null);
        //        newItem.name = gameObject.name;
        //        newItem.GetComponent<ItemHandler>().ReplaceItem(player);
        //        Destroy(gameObject);
         //       return newItem;
        //    }
          //  photonView.TransferOwnership(player.GetPhotonView().ViewID);
            photonView.RPC("RpcParentItem", RpcTarget.All, player.GetPhotonView().ViewID);
            player.GetComponent<AudioSource>().Play();
            player.GetComponent<ChatManager>().LocalNotification("Crafted: " + gameObject.name, Color.blue, true);
            player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().AddToBackpack(gameObject);
            //   player.GetComponent<PlayerReferences>().itemDatabase.GetComponent<ItemDatabase>().AddItemToDatabase(gameObject);
        }
        else
        {
            player.GetComponent<ChatManager>().LocalNotification("You can't carry any more items. Your backpack is full!", Color.red, false);
        }
        return null;
    }


    IEnumerator Expire(float time)
    {
        yield return new WaitForSeconds(time);
        if (photonView.isRuntimeInstantiated)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetAmount(int a)
    {
        amount = a;
        inSlot.UpdateSlotText();
    }

    [PunRPC]
    public void RpcParentItem(int viewId)
    {
        if (viewId == 000)
        {
            gameObject.transform.SetParent(null, true);
            return;
        }
        gameObject.transform.SetParent(PhotonView.Find(viewId).gameObject.GetComponent<PlayerReferences>().itemDatabase.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
    }
    [PunRPC]
    public void RpcInstantiateSceneObject(string itemName, Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.InstantiateSceneObject(itemName, position, rotation, 0, null);
    }

    public void MakeVisibleToAll()
    {
        gameObject.SetActive(false);
        photonView.RPC("RpcInstantiateSceneObject", RpcTarget.MasterClient, gameObject.name, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }
}

[System.Serializable]
public class Combinations
{
    public string combinesWith;
    public string resultName;
    public GameObject resultObject;

}
