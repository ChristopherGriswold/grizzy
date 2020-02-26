using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WalkSound : MonoBehaviour
{
    public AudioSource walkAudioSource;
    public AudioClip walkSound;
    void Start()
    {
        InvokeRepeating("PlaySound", 0.0f, 0.25f);
    }

    void PlaySound()
    {
        if (CrossPlatformInputManager.GetAxis("Vertical") > 0 || CrossPlatformInputManager.GetAxis("Horizontal") > 0)
        {
            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.PlayOneShot(walkSound);
            }
        }
        else
        {
            walkAudioSource.Stop();
        }
    }
}
