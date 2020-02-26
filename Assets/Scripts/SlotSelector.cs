using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSelector : MonoBehaviour
{
    public Slot currentSlotSelected;

    public void ActivateSlotSelector(Slot slot)
    {
        if (!currentSlotSelected)
        {
            currentSlotSelected = slot;
        }

        if(currentSlotSelected != slot)
        {
            currentSlotSelected.GetComponent<Slot>().isSelected = false;
            currentSlotSelected = slot;
        }
        transform.position = slot.gameObject.transform.position;
    }
}
