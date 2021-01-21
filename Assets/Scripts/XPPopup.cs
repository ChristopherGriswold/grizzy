using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XPPopup : MonoBehaviour
{
    public TextMeshPro textMesh;
    public XPController xpController;
    private float textShrinkSpeed;
    private float textGrowSpeed;
    private float textFloatSpeed;
    private bool startShrinking;
    public MeshRenderer meshRenderer;
    public AudioClip echoClip;
    public AudioClip rackClip;
    private GameObject player;
    private bool rackCompleted;
    private int totalXp;
    private int currentXp;
    private AudioSource audioSource;
    private const float SFX_SPEED = .033f;

    private GameObject killerCam;
    private bool isPoppingAny;

    public void Popup(GameObject lookAtTarget, int value)
    {
        totalXp = value;
        meshRenderer.sortingOrder = 3;

        player = lookAtTarget;
        killerCam = player.GetComponentInChildren<Camera>().gameObject;

      //  transform.rotation = Quaternion.LookRotation(transform.position - player.transform.position);
        audioSource = GetComponent<AudioSource>();
        xpController = player.GetComponent<XPController>();
        StopAllCoroutines();
        StartCoroutine("RackupXp");
       // textMesh.text = "+" + value.ToString() + "xp";
    }

    public void Popup(int value)
    {
        Destroy(GameObject.Find("XPPopup (Clone)"));
        isPoppingAny = true;
        totalXp = value;
        meshRenderer.sortingOrder = 3;

        audioSource = GetComponent<AudioSource>();
        StopAllCoroutines();
        StartCoroutine("RackupXpAny");
        // textMesh.text = "+" + value.ToString() + "xp";
    }



    private void Start()
    {
        textGrowSpeed = 10f;
        textShrinkSpeed = 2f;
        //  textFloatSpeed = .01f;
        textFloatSpeed = 1f;
    }
    private void Update()
    {
        if (!isPoppingAny)
        {

            Vector3 lookPos = transform.position - killerCam.transform.position;
            //  lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);

            transform.rotation = rotation;
        }
        if (textMesh.fontSize > 20)
        {
            startShrinking = true;
        }
        if (!startShrinking)
        {
            textMesh.fontSize = (int)Mathf.Lerp(textMesh.fontSize, 30f, textGrowSpeed * Time.deltaTime);
        }
        else
        {
            textMesh.fontSize = (int)Mathf.Lerp(textMesh.fontSize, 12f, textShrinkSpeed * Time.deltaTime);
            if (!isPoppingAny)
            {
                transform.Translate(0, textFloatSpeed * Time.deltaTime, 0);
            }
            else
            {
                transform.Translate(Vector3.right * textFloatSpeed * Time.deltaTime);
            }
        }
    }
    public IEnumerator RackupXpAny()
    {
        int countBy = (int)Mathf.Sqrt(totalXp) - 3;
        if (countBy < 1)
        {
            countBy = 1;
        }
        audioSource.Play();
        audioSource.pitch = 1;
        currentXp = 0;
        while (currentXp < totalXp)
        {
            textMesh.text = "+" + currentXp.ToString() + "xp";
            currentXp += countBy;
            yield return new WaitForSeconds(SFX_SPEED);
        }
        if (currentXp >= totalXp)
        {
            audioSource.Stop();
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(echoClip, 1f);
            textMesh.text = "+" + totalXp.ToString() + "xp";
            Destroy(this.gameObject, 3f);
        }
    }

    public IEnumerator RackupXp()
    {
        int countBy = (int)Mathf.Sqrt(totalXp) -3;
        if (countBy < 1)
        {
            countBy = 1;
        }
        audioSource.Play();
        audioSource.pitch = 1;
        currentXp = 0;
        while (currentXp < totalXp)
        {
            textMesh.text = "+" + currentXp.ToString() + "xp";
         //   xpController.GainXp("Combat", countBy);
            currentXp += countBy;
            yield return new WaitForSeconds(SFX_SPEED);
        }
        if(currentXp >= totalXp)
        {
            audioSource.Stop();
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(echoClip, 1f);
            textMesh.text = "+" + totalXp.ToString() + "xp";
            xpController.GainXp("Attack", totalXp);
            Destroy(this.gameObject, 3f);
        }
    }
}
