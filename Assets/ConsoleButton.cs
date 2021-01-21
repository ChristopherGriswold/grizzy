using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsoleButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public ConsolePanel consolePanel;

    private Vector3 consolePosition;
    public RectTransform consoleRectTransform;


    public void OnDrag(PointerEventData eventData)
    {
     //   consolePosition.y += eventData.delta.y;
     //   consolePosition.y = Mathf.Clamp(consolePosition.y, 720f, 864f);
     //   consoleRectTransform.transform.position = consolePosition;
    }

    public void ClickedButton()
    {
        
        if (!consolePanel.isOpen)
        {
            Vector3 chatPanelPos = consoleRectTransform.localPosition;
            chatPanelPos.y = 0;
            consoleRectTransform.localPosition = chatPanelPos;

            consolePanel.StopAllCoroutines();
            consolePanel.leaveOpen = true;
            StartCoroutine(consolePanel.OpenConsole());
        }
        else
        {
            consolePanel.StopAllCoroutines();
            consolePanel.leaveOpen = false;
            StartCoroutine(consolePanel.CloseConsole());
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    void Awake()
    {
        consolePosition = consolePanel.gameObject.transform.position;
     //   consoleRectTransform = consolePanel.GetComponent<RectTransform>();
    }
}
