using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("Elements")] 
    public GameObject questPanel;
    public ItemToolTip toolTip;
    private bool isOpen;
    
    [Header("Quest Name")]
    public RectTransform questListTransform;
    public QuestNameButton questNameButton;
    
    [Header("Text Content")]
    public Text questContextText;

    [Header("Requirement")]
    public RectTransform requiRectTransform;
    public QuestRequirement requirement;

    [Header("Reward Panel")]
    public RectTransform rewardTransform;
    public ItemUI rewardUI;

    private void Update()
    {
        //按下Q键打开（关闭）面板
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;
            questPanel.SetActive(isOpen);
            if (!isOpen)
            {
                //关闭任务面板则关闭物品描述
                toolTip.gameObject.SetActive(false);
            }
            questContextText.text = string.Empty;
            //显示面板内容
            setupQuestList();
        }
    }

    //设置任务面板内容
    public void setupQuestList()
    {
        //首先清空所有内容，为填入新内容做准备
        foreach (Transform item in questListTransform)
        {
            Destroy(item.gameObject);
        }
        
        foreach (Transform item in rewardTransform)
        {
            Destroy(item.gameObject);
        }
        
        foreach (Transform item in requiRectTransform)
        {
            Destroy(item.gameObject);
        }
        //创建新的任务列表
        foreach (var task in QuestManager.Instance.tasks)
        {
            var newTask = Instantiate(questNameButton, questListTransform);
            newTask.SetupNameButton((task.questData));
            newTask.questContentText = questContextText;
        }
    }

    //设置任务需求列表
    public void SetupRequireList(QuestData_SO questData)
    {
        foreach (Transform item in requiRectTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var require in questData.questRequires)
        {
            var q = Instantiate(requirement, requiRectTransform);
            if (questData.isFinished)
            {
                q.SetupRequirement(require.name,questData.isFinished);
            }
            else
            {
                q.SetupRequirement(require.name,require.requireAmount,require.currentAmount);
            }
            q.SetupRequirement(require.name,require.requireAmount,require.currentAmount);
        }
    }
    
    //设置奖励列表
    public void SetupRewardItem(ItemData_SO itemData, int amount)
    {
        var item = Instantiate(rewardUI, rewardTransform);
        item.SetupItemUI(itemData,amount);
    }
}
