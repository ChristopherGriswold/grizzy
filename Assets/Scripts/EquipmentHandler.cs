using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EquipmentHandler : MonoBehaviourPun
{
    public GameObject onFloor;
    public Vector3 equipPosition;
    public Vector3 equipRotation;

    public void Equip2()
    {
        photonView.RPC("RpcEquip", RpcTarget.AllBuffered, null);  //This needs to be optimized. Buffer needs to be removed upon unequip.
    }
    public void Unequip()
    {
        photonView.RPC("RpcUnequip", RpcTarget.All, null);
        photonView.RPC("RpcTellMasterToClearRpc", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
    }


    [PunRPC]
    public void RpcTellMasterToClearRpc(Photon.Realtime.Player player)
    {
        PhotonNetwork.RemoveRPCs(player);    
    }

  //  [PunRPC]
    public void Equip()
    {
        this.gameObject.transform.root.GetComponentInChildren<CustomTouchPad>().RegisterWeapon(this.gameObject);
    //    gameObject.layer = 13;
        onFloor.SetActive(false);
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<Light>().enabled = false;
        transform.localPosition = equipPosition;
        transform.localRotation = Quaternion.Euler(equipRotation.x, equipRotation.y, equipRotation.z);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition; //QUESTIONABLE!!!
        GetComponent<ItemHandler>().isEquipped = true;
        this.gameObject.SetActive(true);
    }


    [PunRPC]
    public void RpcUnequip()
    {
        gameObject.layer = 14;
        onFloor.SetActive(true);
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<Light>().enabled = true;
        transform.localPosition = equipPosition;

        transform.localRotation = Quaternion.Euler(equipRotation.x, equipRotation.y, equipRotation.z);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePosition; //QUESTIONABLE!!!
        GetComponent<ItemHandler>().isEquipped = false;
    }

}
