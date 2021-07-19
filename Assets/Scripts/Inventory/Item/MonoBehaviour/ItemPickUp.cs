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
            
            //装备武器
            
            //销毁被拾取物品
            Destroy(gameObject);
        }
    }
}
