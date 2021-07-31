using System.Collections;
using System.Collections.Generic;
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
}
