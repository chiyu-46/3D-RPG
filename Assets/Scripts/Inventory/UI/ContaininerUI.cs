using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContaininerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;

    public void RefreshUI()
    {
        for (int i = 0; i < slotHolders.Length; i++)
        {
            slotHolders[i].itemUI.Index = i;
            slotHolders[i].UpdateItem();
        }

        
    }
}
