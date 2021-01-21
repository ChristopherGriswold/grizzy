using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public int itemId;
    public string itemName;
    public string description;
    public string type;
    public string useButtonText;
    public int xpToCraft;
    public string effect;
    public int effectInt;
    public int value;
    public int amount = 1;
    public int randomMinAmount;
    public int randomMaxAmount;
    public int levelRequired;
    public int maxDrawPower;
    public bool isKeptOnDeath;
    public bool isStackable;
    public bool isStackableInBank;
    public bool isQuestItem;
    public bool shouldExpire;
    public bool shouldBecomeVisibleToAll;
    public bool isEquipped;
    public Slot inSlot;
    public GameObject triggerCollider;
    public bool doGetGatheringXp;
    public int gatheringXp;

    //key = name of item this item can be combined with. Value = name of item that is created upon combining said items.
    public List<Combinations> combinationsList = new List<Combinations>();

    public AudioClip useItemSound;

    // Use this for initialization
    void Awake ()
    {

        gameObject.name = itemName;
        if (randomMinAmount > 0 )
        {
            amount = Random.Range(randomMinAmount, randomMaxAmount + 1);
        }

        if (value == 0)
        {
            gameObject.GetComponent<Light>().color = Color.red;
        }else if (value >= 50)
        {
            gameObject.GetComponent<Light>().color = Color.magenta;
        }else if (value >= 10)
        {
            gameObject.GetComponent<Light>().color = Color.blue;
        }else if (value >= 2)
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
        if(collision.gameObject.layer == 25)
        {
            Vector3 directionToPlayer = collision.gameObject.transform.position - transform.position;
            gameObject.GetComponent<Rigidbody>().AddForce(directionToPlayer * 10f);
        }
        if (this.GetComponentInChildren<BoxCollider>())
        {
         //   return;
        }
        if (collision.gameObject.layer == 13)
        { 
            PickUpItem(collision.gameObject);
        }
    }

    public void PickUpItem(GameObject player)
    {
        if (player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().RoomInBackpack(this.gameObject))
        {
            triggerCollider.SetActive(false);
            gameObject.SetActive(false);
            StopAllCoroutines();
            transform.SetParent(player.GetComponent<PlayerReferences>().itemDatabase.transform);
            player.GetComponent<AudioSource>().Play();
            player.GetComponent<ChatManager>().LocalNotification(gameObject);
            if (doGetGatheringXp)
            {
                player.GetComponent<XPController>().GainXp("Gathering", gatheringXp);
                doGetGatheringXp = false;
            }
            player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().AddToBackpack(gameObject);
        }
        else
        {
            player.GetComponent<ChatManager>().LocalNotification("You can't carry any more items. Your backpack is full!", Color.red, false);
        }

    }

    public void ReceiveItem(GameObject player)
    {
        if (player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().RoomInBackpack(this.gameObject))
        {
            triggerCollider.SetActive(false);
            gameObject.SetActive(false);
            StopAllCoroutines();
            transform.SetParent(player.GetComponent<PlayerReferences>().itemDatabase.transform);
            player.GetComponent<AudioSource>().Play();
            player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().AddToBackpack(gameObject);
        }
        else
        {
            player.GetComponent<ChatManager>().LocalNotification("You can't carry any more items. Your backpack is full!", Color.red, false);
        }
    }

    public void CraftItem(GameObject player)
    {
        if(type == "Weapon")
        {
            amount = 0;
        }
        triggerCollider.SetActive(false);
        gameObject.SetActive(false);
        StopAllCoroutines();
        transform.SetParent(player.GetComponent<PlayerReferences>().itemDatabase.transform);
        player.GetComponent<AudioSource>().Play();
        player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().AddToBackpack(gameObject);
        player.GetComponent<ChatManager>().LocalNotification("Crafted: " + gameObject.name, Color.green, true);
        player.GetComponent<XPController>().GainXp("Crafting", xpToCraft);

    }



        public void DropItem()
    {
        GameObject player = gameObject.transform.root.gameObject;
        if (isEquipped)
        {
            this.gameObject.GetComponent<EquipmentHandler>().Unequip();
        }
        Vector3 originalPos = gameObject.transform.parent.transform.position;
        Quaternion originalRot = gameObject.transform.parent.transform.rotation;
        transform.SetParent(null);
        transform.position = originalPos;
        transform.rotation = originalRot;
        Physics.IgnoreLayerCollision(13, 20, true);
        Physics.IgnoreLayerCollision(14, 14, true);
        gameObject.SetActive(true);
        StartCoroutine(TriggerDisabledTime());
        GetComponent<Rigidbody>().AddRelativeForce(Random.Range(-10, 10), Random.Range(10, 25), Random.Range(10, 25));
        triggerCollider.SetActive(true);
        player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().RemoveItem(gameObject);
        player.GetComponent<ChatManager>().LocalNotification("Dropped: " + gameObject.name, Color.red, true);
    }

    IEnumerator TriggerDisabledTime()
    {
        yield return new WaitForSeconds(1f);
        Physics.IgnoreLayerCollision(13, 20, false);
        Physics.IgnoreLayerCollision(14, 14, true);
    }

    public GameObject ReplaceItem(GameObject player)
    {
        if (player.GetComponent<PlayerReferences>().backpack.GetComponent<Backpack>().RoomInBackpack())
        {
            StopAllCoroutines();
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

    public void SetAmount(int a)
    {
        amount = a;
        inSlot.UpdateSlotText();
    }

    public void RpcParentItem(int viewId)
    {
        if (viewId == 000)
        {
            gameObject.transform.SetParent(null, true);
            return;
        }
        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>().itemDatabase.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
    }

}

[System.Serializable]
public class Combinations
{
    public string combinesWith;
    public string resultName;
    public GameObject resultObject;

}
