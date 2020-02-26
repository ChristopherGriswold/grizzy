using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public bool menuOpen;
    public GameObject menu;
    public GameObject itemDetails;
    public GameObject itemDatabase;
    public GameObject slotSelector;
    public AudioClip openBackpackSound;
    public AudioClip closeBackpackSound;

    private AudioSource aud;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    public void OpenMenu()
    {
        CancelInvoke("RestoreCollisions");
        aud.PlayOneShot(openBackpackSound);
        Physics.IgnoreLayerCollision(13, 14, true);
        menuOpen = true;
        menu.SetActive(true);
    }
    public void CloseMenu()
    {
        aud.PlayOneShot(closeBackpackSound);
        Invoke("RestoreCollisions", 1f);
        menuOpen = false;
        menu.SetActive(false);
    }

    void RestoreCollisions()
    {
        Physics.IgnoreLayerCollision(13, 14, false);
    }
}
