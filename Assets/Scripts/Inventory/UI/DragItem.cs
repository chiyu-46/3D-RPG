using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ItemUI currentItemUI;
    private SlotHolder currentHolder;
    private SlotHolder targetHolder;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        //记录原始信息，无法移动放回原始状态
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform,true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //跟随鼠标
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品，交换数据
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheakInActionUI(eventData.position) 
                || InventoryManager.Instance.CheakInEquipmentUI(eventData.position)
                || InventoryManager.Instance.CheakInInventoryUI(eventData.position))
            {
                targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                if (!targetHolder)
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }

                if (targetHolder != InventoryManager.Instance.currentDrag.originalHolder)
                {
                    switch (targetHolder.slotType)
                    {
                        case SlotType.BAG:
                            SwapItem();
                            break;
                        case SlotType.WEAPON:
                            if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Weapon)
                            {
                                SwapItem();
                            }
                            break;
                        case SlotType.ARMOR:
                            if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Armor)
                            {
                                SwapItem();
                            }
                            break;
                        case SlotType.ACTION:
                            if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Useable)
                            {
                                SwapItem();
                            }
                            break;
                    }
                }
                
                
                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
            }
        }
        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
        RectTransform t = transform as RectTransform;
        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }

    public void SwapItem()
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];

        bool isSameItem = tempItem.itemData == targetItem.itemData;

        if (isSameItem && targetItem.itemData.stackable)
        {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }
    }
}
