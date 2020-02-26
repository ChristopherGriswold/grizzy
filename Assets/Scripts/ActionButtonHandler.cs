using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButtonHandler : MonoBehaviour, IPointerExitHandler, IPointerDownHandler
{

    private AudioSource clickSoundSource;
    public Text buttonText;
    public AudioClip clickSomething;
    public AudioClip clickNothing;

	void Start ()
    {
        clickSoundSource = gameObject.GetComponent<AudioSource>();
	}


    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(buttonText.text == "")
        {
            clickSoundSource.clip = clickNothing;
        }
        else
        {
            clickSoundSource.clip = clickSomething;
        }
        clickSoundSource.Play();
    }
}
