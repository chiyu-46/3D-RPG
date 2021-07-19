using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("Inventory Data")]
    public InventoryData_SO inventoryData;

    [Header("Containers")] 
    public ContaininerUI inventoryUI;

    private void Start()
    {
        inventoryUI.RefreshUI();
    }
}
