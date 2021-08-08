using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO:拾取物品，添加到背包
            InventoryManager.Instance.inventoryData.AddItem(itemData,itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //装备武器
            //GameManager.Instance.playerStats.EquipWeapon(itemData);
            //检查是否能推进任务
            QuestManager.Instance.UpdateQuestProgress(itemData.itemName,itemData.itemAmount);
            //销毁被拾取物品
            Destroy(gameObject);
        }
    }
}
