using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePlayer : MonoBehaviour
{
    public GameObject bloodScreenOverlay;
    public HealthController healthController;
    public void TakeDamage(int amount)
    {
        StopAllCoroutines();
        StartCoroutine(BloodOnScreen());
        healthController.LoseHealth(amount);
    }

    private void OnCollisionEnter(Collision collision)
    {
    //    Debug.Log("COllided with player");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();
    }
   public IEnumerator BloodOnScreen()
    {
        bloodScreenOverlay.SetActive(true);
        float alpha = 1.0f; //1 is opaque, 0 is transparent
        RawImage bloodScreenImage = bloodScreenOverlay.GetComponent<RawImage>();
        Color currColor = bloodScreenImage.color;
        while (alpha > 0f)
        {
            alpha -= .025f;
            currColor.a = alpha;
            yield return new WaitForEndOfFrame();
            bloodScreenImage.color = currColor;
        }
        bloodScreenOverlay.SetActive(false);
    }
}
