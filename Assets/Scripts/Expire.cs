using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expire : MonoBehaviour
{
    public float expireTimer;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, expireTimer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
