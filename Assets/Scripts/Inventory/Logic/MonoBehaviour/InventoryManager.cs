using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }
    [Header("Inventory Data")]
    public InventoryData_SO inventoryDataTemplate;
    public InventoryData_SO actionDataTemplate;
    public InventoryData_SO equipmentDataTemplate;
    //以下三个时上面三个的副本,以上三个为模板
    public InventoryData_SO inventoryData;
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentData;

    [Header("Containers")] 
    public ContaininerUI inventoryUI;
    public ContaininerUI actionUI;
    public ContaininerUI equipmentUI;

    [Header("Drag Canvas")] 
    public Canvas dragCanvas;
    public DragData currentDrag;

    [Header("UI Panel")]
    public GameObject bagPanel;
    public GameObject statesPanel;
    private bool isOpen = false;//此面板是否处于打开状态

    [Header("States Text")] 
    public Text healthText;
    public Text attackText;

    [Header("ToolTip")] 
    public ItemToolTip toolTip;

    protected override void Awake()
    {
        base.Awake();
        //从模板数据实例化真正使用的数据
        if (!inventoryDataTemplate)
        {
            inventoryData = Instantiate(inventoryDataTemplate);
        }
        if (!actionDataTemplate)
        {
            actionData = Instantiate(actionDataTemplate);
        }
        if (!equipmentDataTemplate)
        {
            equipmentData = Instantiate(equipmentDataTemplate);
        }
    }

    private void Start()
    {
        LoadData();
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }

    private void Update()
    {
        //按B键，打开或关闭此面板
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statesPanel.SetActive(isOpen);
        }
        UpdateStatesText(GameManager.Instance.playerStats.CurrentHealth, GameManager.Instance.playerStats.minDamge,
            GameManager.Instance.playerStats.maxDamge);
    }

    public void UpdateStatesText(int health, int min, int max)
    {
        healthText.text = health.ToString();
        attackText.text = min + "-" + max;
    }

    #region Save&Load

    public void SaveData()
    {
        SaveManager.Instance.Save(inventoryData,inventoryData.name);
        SaveManager.Instance.Save(actionData,actionData.name);
        SaveManager.Instance.Save(equipmentData,equipmentData.name);
    }

    public void LoadData()
    {
        SaveManager.Instance.Load(inventoryData,inventoryData.name);
        SaveManager.Instance.Load(actionData,actionData.name);
        SaveManager.Instance.Load(equipmentData,equipmentData.name);
    }

    #endregion
    #region 检查拖拽物品是否在每一个Slot范围内

    public bool CheakInInventoryUI(Vector3 position)
    {
        for (int i = 0; i < inventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = inventoryUI.slotHolders[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t,position))
            {
                return true;
            }
        }

        return false;
    }

    public bool CheakInActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform t = actionUI.slotHolders[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t,position))
            {
                return true;
            }
        }

        return false;
    }
    
    public bool CheakInEquipmentUI(Vector3 position)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Length; i++)
        {
            RectTransform t = equipmentUI.slotHolders[i].transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(t,position))
            {
                return true;
            }
        }

        return false;
    }
    #endregion

    #region 检测任务物品

    //遍历背包，确定当前是否存在任务物品，存在则刷新任务进度。
    public void CheckQuestItemInBug(string questItemName)
    {
        foreach (var item in inventoryData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == questItemName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName,item.amount);
                }
            }
        }
        
        foreach (var item in actionData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == questItemName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName,item.amount);
                }
            }
        }
    }

    #endregion

    #region 检测背包和快捷栏中的任务物品

    public InventoryItem QuestItemInBag(ItemData_SO questItem)
    {
        return inventoryData.items.Find(i => i.itemData == questItem);
    }
    public InventoryItem QuestItemInAction(ItemData_SO questItem)
    {
        return actionData.items.Find(i => i.itemData == questItem);
    }

    #endregion
}
