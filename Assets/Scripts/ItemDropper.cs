using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{

    public GameObject[] itemsToDrop = new GameObject[5];
    // Start is called before the first frame update
    private void Awake()
    {
        foreach (GameObject item in itemsToDrop)
        {
            GameObject newItem = (GameObject)Instantiate(Resources.Load(item.name), gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
