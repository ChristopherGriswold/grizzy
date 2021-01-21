using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private FireWeapon fireWeapon;
    private MeshRenderer meshRenderer;
    public float scrollSpeed = 0.5f;
    public AudioClip zapSound;
    public AudioSource audioSource;
    private void OnTriggerEnter(Collider other)
    {
        MakeSafe(other);
    }

    void MakeSafe(Collider other)
    {
        if (other.gameObject.layer == 15)
        {
            audioSource.clip = zapSound;
            audioSource.Play();
            other.gameObject.GetComponentInChildren<EnemyController>().PhaseOut();
        }
        if (other.gameObject.layer == 23)
        {
            if (other.GetComponentInParent<FireWeapon>())
            {
                other.gameObject.GetComponentInParent<FireWeapon>().canShoot = false;

                if (other.gameObject.GetComponentInParent<FireWeapon>().bowDrawn)
                {
                    other.gameObject.GetComponentInParent<FireWeapon>().CancelDrawBow();
                }
            }
        }
        /* if (other.gameObject.layer == 13)
         {
             if(fireWeapon == null)
             {
                 fireWeapon = other.gameObject.GetComponentInChildren<FireWeapon>();
             }
             if (fireWeapon)
             {
                 fireWeapon.canShoot = false;
                 if (fireWeapon.bowDrawn)
                 {
                     fireWeapon.CancelDrawBow();
                 }
             }
         }*/
    }

    private void OnTriggerStay(Collider other)
    {
        //    MakeSafe(other);
        if (other.gameObject.layer == 15)
        {
            audioSource.clip = zapSound;
            audioSource.Play();
            other.gameObject.GetComponent<Collider>().enabled = false;
            other.gameObject.GetComponentInChildren<EnemyController>().PhaseOut();
        }
        if (other.gameObject.layer == 23)
        {
            if (other.gameObject.GetComponentInParent<FireWeapon>())
            {
                other.gameObject.GetComponentInParent<FireWeapon>().canShoot = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 23)
        {
            if (other.gameObject.GetComponentInParent<FireWeapon>())
            {
                other.gameObject.GetComponentInParent<FireWeapon>().canShoot = true;
            }
        }

        if (other.gameObject.layer == 15)
        {
            if(other.gameObject.GetComponent<AgentMove>().staysAggressive == true)
            {
                other.gameObject.GetComponent<AgentMove>().isAggressive = true;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Time.time * scrollSpeed;
        meshRenderer.material.mainTextureOffset = new Vector2(0, offset);

        transform.Rotate(Vector3.up * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * Time.deltaTime, Space.World);
    }
}
