using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentHandler : MonoBehaviour
{
    public GameObject onFloor;
    public GameObject model;
    public Vector3 equipPosition;
    public Vector3 equipRotation;



    public void Equip()
    {
        this.gameObject.transform.root.GetComponentInChildren<CustomTouchPad>().RegisterWeapon(this.gameObject);
    //    gameObject.layer = 13;
        onFloor.SetActive(false);
        model.SetActive(true);
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<Light>().enabled = false;
        transform.localPosition = equipPosition;
        transform.localRotation = Quaternion.Euler(equipRotation.x, equipRotation.y, equipRotation.z);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition; //QUESTIONABLE!!!
        GetComponent<ItemHandler>().isEquipped = true;
        this.gameObject.SetActive(true);
    }

    public void Unequip()
    {
        this.gameObject.transform.root.GetComponentInChildren<CustomTouchPad>().fireWeapon = null;
        gameObject.layer = 14;
        onFloor.SetActive(true);
        model.SetActive(false);
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<Light>().enabled = true;
        transform.localPosition = equipPosition;

        transform.localRotation = Quaternion.Euler(equipRotation.x, equipRotation.y, equipRotation.z);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePosition; //QUESTIONABLE!!!
        GetComponent<ItemHandler>().isEquipped = false;
    }

}
