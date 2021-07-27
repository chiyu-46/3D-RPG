using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType{ BAG, WEAPON, ARMOR, ACTION}

public class SlotHolder : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemUI itemUI;
    public SlotType slotType;

    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                //装备武器，切换武器
                if (itemUI.Bag.items[itemUI.Index].itemData != null)
                {
                    GameManager.Instance.playerStats.ChangeWeapon(itemUI.Bag.items[itemUI.Index].itemData);
                }
                else
                {
                    GameManager.Instance.playerStats.UnEquiWeapon();
                }
                break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
        }

        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetupItemUI(item.itemData,item.amount);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount >= 2)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (!itemUI.GetItem())
        {
            return;
        }
        if (itemUI.GetItem().itemType == ItemType.Useable && itemUI.Bag.items[itemUI.Index].amount > 0)
        {
            GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().itemData.healthPoint);
            itemUI.Bag.items[itemUI.Index].amount -= 1;
        }
        UpdateItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem())
        {
            InventoryManager.Instance.toolTip.SetupTooltip(itemUI.GetItem());
            InventoryManager.Instance.toolTip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);
    }
}
