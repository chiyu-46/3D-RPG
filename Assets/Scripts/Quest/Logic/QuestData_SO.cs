using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    //定义任务内容(此套方案使用名称检测任务进度，要求任务目标的名称相同，不能使用xxx01这类)
    [System.Serializable]
    public class QuestRequire
    {
        public string name;             //任务需要收集（消灭）名字
        public int requireAmount;       //任务需要收集（消灭）数量
        public int currentAmount;       //已进行的数量
    }
    
    public string questName;            //任务名称
    [TextArea] 
    public string description;          //任务描述

    public bool isStarted;              //任务开始
    public bool isComplete;             //任务完成（可领取奖励的状态）
    public bool isFinished;             //任务结束（奖励已领取的状态）

    //存在同一任务需要多个完成条件的情况，实际任务需求为列表
    public List<QuestRequire> questRequires = new List<QuestRequire>();
    //任务奖励列表
    public List<InventoryItem> rewards = new List<InventoryItem>();
    
    //检测任务进度
    public void CheckQuestProgress()
    {
        var finishRequires = questRequires.Where(r => r.requireAmount <= r.currentAmount);
        isComplete = finishRequires.Count() == questRequires.Count;
    }
    
    //获取当前任务需要 消灭/收集 目标的名字列表
    public List<string> RequireTargetName()
    {
        List<string> tarfetNameList = new List<string>();
        foreach (var require in questRequires)
        {
            tarfetNameList.Add(require.name);
        }

        return tarfetNameList;
    }
    
    //给奖励
    public void GiveRewards()
    {
        foreach (var reward in rewards)
        {
            if (reward.amount < 0)//需要上交任务物品的情况
            {
                int requireCount = Mathf.Abs(reward.amount);

                if (InventoryManager.Instance.QuestItemInBag(reward.itemData) != null)//背包当中有需要交的物品
                {
                    if (InventoryManager.Instance.QuestItemInBag(reward.itemData).amount <= requireCount)//背包当中需要上交物品的数量刚好够或者不够的情况
                    {
                        requireCount -= InventoryManager.Instance.QuestItemInBag(reward.itemData).amount;
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount = 0;

                        if (InventoryManager.Instance.QuestItemInAction(reward.itemData) != null)
                            InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
                    }
                    else//背包当中上交物品的数量充足
                    {
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount -= requireCount;
                    }
                }
                else//背包当中没有上交物品代表Action中一定满足了任务物品的数量
                {
                    InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
                }
            }
            else//正常获得的额外物品奖励添加到背包中
            {
                InventoryManager.Instance.inventoryData.AddItem(reward.itemData, reward.amount);
            }

            InventoryManager.Instance.inventoryUI.RefreshUI();
            InventoryManager.Instance.actionUI.RefreshUI();
        }
    }
}
