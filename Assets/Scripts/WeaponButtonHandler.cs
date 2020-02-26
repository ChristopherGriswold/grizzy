using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButtonHandler : MonoBehaviour
{
    public GameObject equippedWeapon;
    public GameObject target;
    public GameObject player;

    private FireWeapon fireWeapon;
    private WeaponHandler weaponHandler;

    public void RegisterEquippedWeapon(GameObject weapon)
    {
        equippedWeapon = weapon;
        if (!equippedWeapon)
        {
            return;
        }
        equippedWeapon.GetComponent<FireWeapon>().enabled = true;
        fireWeapon = equippedWeapon.GetComponent<FireWeapon>();
        weaponHandler = equippedWeapon.GetComponent<WeaponHandler>();
    }
    public void RegisterTarget(GameObject t)
    {
        if (!equippedWeapon)
        {
            return;
        }
        target = t;
        weaponHandler.targetedEnemy = t;
    }
    public void PressShootButton()
    {
        if (!equippedWeapon)
        {
            return;
        }
        if (target)
        {
            fireWeapon.FireShot(target);
        }
        else
        {
            fireWeapon.FireShot();
        }
    }
}
