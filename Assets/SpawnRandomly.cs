using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomly : MonoBehaviour
{

    private SpawnItems spawnItems;



    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13)
        {
            spawnItems = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<SpawnItems>();
            spawnItems.SpawnOne((GameObject)Resources.Load(gameObject.name));
        }
    }


}
