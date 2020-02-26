using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsolePanel : MonoBehaviour
{
    public bool isOpen;
    public bool leaveOpen;
    public IEnumerator DisplayConsole()
    {
        StartCoroutine(OpenConsole());
        yield return new WaitForSeconds(5f);
        StartCoroutine(CloseConsole());
    }

    public IEnumerator OpenConsole()
    {

      //  StopCoroutine(OpenConsole());
    //    StopCoroutine(CloseConsole());
        isOpen = true;
        float t = 0;
        RectTransform rectTransoform = this.gameObject.GetComponent<RectTransform>();
        Vector2 consolePanelPosition = new Vector2(640, rectTransoform.transform.position.y);
        while (rectTransoform.transform.position.y > 720)
        {
            rectTransoform.transform.position = new Vector2(consolePanelPosition.x, Mathf.Lerp(864f, 720f, t));
            t += Time.deltaTime * 2f;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator CloseConsole()
    {
        if (leaveOpen)
        {
            yield break;
        }
     //   StopCoroutine(OpenConsole());
       // StopCoroutine(CloseConsole());
        isOpen = false;
        float t = 0;

        RectTransform rectTransoform = this.gameObject.GetComponent<RectTransform>();
        Vector2 consolePanelPosition = new Vector2(640, rectTransoform.transform.position.y);
        while (rectTransoform.transform.position.y < 864)
        {
            rectTransoform.transform.position = new Vector2(consolePanelPosition.x, Mathf.Lerp(720f, 864f, t));
            t += Time.deltaTime * 2f;
            yield return new WaitForEndOfFrame();
        }
    }

    public void StartDisplay()
    {
        StopAllCoroutines();
        if (leaveOpen)
        {
            return;
        }
        StartCoroutine(DisplayConsole());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
