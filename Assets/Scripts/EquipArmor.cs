using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipArmor : MonoBehaviour
{
    public GameObject onFloor;
    public Vector3 equipPosition;

    public void Equip()
    {
        gameObject.layer = 13;
        onFloor.SetActive(false);
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<Light>().enabled = false;
        transform.localPosition = equipPosition;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<ItemHandler>().isEquipped = true;
    }
    public void Unequip()
    {
        gameObject.layer = 14;
        onFloor.SetActive(true);
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<Light>().enabled = true;
        transform.localPosition = equipPosition;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<ItemHandler>().isEquipped = false;
    }
}
