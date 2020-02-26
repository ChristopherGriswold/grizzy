using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFlight : MonoBehaviour
{
    public Rigidbody rigidBody;
    public BoxCollider boxCollider;
    public GameObject particles;
    public GameObject arrowTrailParticles;
    public GameObject bloodParticles;
    public GameObject hitParticles;
    public GameObject hitMarker;
    private float velocity;
    private GameObject _shooter;
    private FireWeapon _fireWeapon;
    private bool hasHit;
    private ParticleSystem bloodParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateArrowCollider());
       bloodParticleSystem = bloodParticles.GetComponent<ParticleSystem>();
        Destroy(this.gameObject, 60f);
    }
    private IEnumerator ActivateArrowCollider()
    {
        yield return new WaitForSeconds(0f);
        Time.fixedDeltaTime = 0.01f;
        boxCollider.enabled = true;
        arrowTrailParticles.SetActive(true);
        yield break;

    }

    private void OnTriggerEnter(Collider collision)
    {
        Time.fixedDeltaTime = 0.02f;
        hasHit = true;
        rigidBody.isKinematic = true;
        particles.transform.SetParent(null);
        ParticleSystem ps = arrowTrailParticles.GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule isPs = ps.emission;
        isPs.enabled = false;
        if (collision.gameObject.layer == 15)
        {
            bloodParticles.SetActive(true);

            var main = bloodParticleSystem.main;
            main.loop = false;   // prewarm only works on looping systems
        }
        else
        {
            hitParticles.SetActive(true);
            hitMarker.SetActive(true);
        }
        Transform newParent = collision.transform;
        this.gameObject.transform.SetParent(newParent);

        if (newParent.gameObject.layer == 15)
        {
            // newParent.GetComponent<EnemyController>().Damage(1000, _shooter);
            _fireWeapon.InflictDamage(newParent.gameObject);
            _fireWeapon.ForceAcquireTarget(newParent.gameObject);
        }
        boxCollider.enabled = false;

      //  this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasHit)
        {
            return;
        }
        transform.rotation = Quaternion.LookRotation(rigidBody.velocity);
    }

    public GameObject GetShooter()
    {
        return _shooter;
    }

    public void SetShooter(GameObject shooter)
    {
        _shooter = shooter;
    }

    public void SetFireWeapon(FireWeapon fireWeapon)
    {
        _fireWeapon = fireWeapon;
    }
}
