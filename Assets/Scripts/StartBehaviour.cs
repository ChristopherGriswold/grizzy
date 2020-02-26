using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBehaviour : MonoBehaviour {
    public GameObject startDirectionsParent;

    // Use this for initialization

    private void Awake()
    {
        Time.timeScale = 0;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1;
        Destroy(startDirectionsParent);
    }
	
	// Update is called once per frame
	void Update ()
    {

	}
}
