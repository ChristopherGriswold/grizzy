using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingManager : MonoBehaviour
{
    private Backpack backpack;
    public void CookAll()
    {
       backpack = transform.root.gameObject.GetComponentInChildren<Backpack>(true);
       Cookable[] cookableItems = transform.root.gameObject.GetComponentsInChildren<Cookable>(true);

        for(int i = 0; i < cookableItems.Length; i++)
        {
            cookableItems[i].Cook();
        }
        backpack.SetBackpack(false);
        cookableItems = null;
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}
