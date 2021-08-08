using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    public Text questNameText;
    public QuestData_SO currentData;
    public Text questContentText;               //任务详细文本框，由QuestUI传入

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    //点击任务名称按钮时执行，更新面板右侧显示内容
    void UpdateQuestContent()
    {
        questContentText.text = currentData.description;
        QuestUI.Instance.SetupRequireList(currentData);
        foreach (Transform item in QuestUI.Instance.rewardTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in currentData.rewards)
        {
            QuestUI.Instance.SetupRewardItem(item.itemData,item.amount);
        }
    }

    //设置任务名称按钮的文本内容
    public void SetupNameButton(QuestData_SO questData)
    {
        currentData = questData;
        if (questData.isComplete)
        {
            questNameText.text = questData.questName + "(完成)";
        }
        else
        {
            questNameText.text = questData.questName;
        }
    }
}
