using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowToolTip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private ItemUI currentItemUI;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        QuestUI.Instance.toolTip.gameObject.SetActive(true);
        QuestUI.Instance.toolTip.SetupTooltip(currentItemUI.currentItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        QuestUI.Instance.toolTip.gameObject.SetActive(false);
    }
}
