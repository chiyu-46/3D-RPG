using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;
    public Text amount = null;

    public InventoryData_SO Bag { get; set; }
    public int Index { get; set; } = -1;
    
    public void SetupItemUI(ItemData_SO item, int itemAmount)
    {
        //判断物品持有数量是否到达零
        if (itemAmount == 0)
        {
            Bag.items[Index].itemData = null;
            icon.gameObject.SetActive(false);
            return;
        }
        if (item)
        {
            icon.sprite = item.itemIcon;
            amount.text = itemAmount.ToString();
            icon.gameObject.SetActive(true);
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }

    public ItemData_SO GetItem()
    {
        return Bag.items[Index].itemData;
    }
}
