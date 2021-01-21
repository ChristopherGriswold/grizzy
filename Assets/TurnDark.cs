using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDark : MonoBehaviour
{
    private float fogStart;
    private float fogEnd;
    private bool isDark;

    private void Awake()
    {
        fogStart = RenderSettings.fogStartDistance;
        fogEnd = RenderSettings.fogEndDistance;
    }


    private void OnTriggerStay(Collider other)
    {
        if(!isDark && other.gameObject.layer == 13)
        {
            GoDark();
        }
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13)
        {
            GoDark();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            RenderSettings.fogStartDistance = fogStart;
            RenderSettings.fogEndDistance = fogEnd;
        }
        isDark = false;
    }

    private void GoDark()
    {
        RenderSettings.fogStartDistance = 1f;
        RenderSettings.fogEndDistance = 50f;
        isDark = true;
    }

}
