using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public GameObject targetedEnemy;
    public float resetWeaponSpeed;
    private Quaternion targetRotation;

    void Update ()
	{
        if (targetedEnemy && targetedEnemy.activeInHierarchy)
        {
            targetRotation = Quaternion.LookRotation(targetedEnemy.transform.GetChild(0).position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, resetWeaponSpeed * 3 * Time.deltaTime);
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(Vector3.zero), resetWeaponSpeed * Time.deltaTime);
        }
    }
}
