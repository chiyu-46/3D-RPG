using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;
    private Button thisButton;
    public DialoguePiece currentPiece;
    private string nextPieceID;
    private bool takeQuest;                             //是否包含任务
    
    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClicked);
    }

    public void OnOptionClicked()
    {
        //此处两个判断，第一层判断NPC是否想让我们接受任务，第二次判断我们是否决定接受任务（比如选择现在没空）
        if (currentPiece.quest != null)
        {
            //NPC提供任务，则创建实际使用的任务副本
            var newTask = new QuestManager.QuestTask
            {
                questData = Instantiate(currentPiece.quest)
            };
            //判断是否是接受任务的选项
            if (takeQuest)
            {
                if (QuestManager.Instance.HaveQuest(newTask.questData))
                {
                    //已有此任务则领取奖励
                    if (QuestManager.Instance.GetTask(newTask.questData).IsComplete)
                    {
                        newTask.questData.GiveRewards();
                        QuestManager.Instance.GetTask(newTask.questData).IsFinished = true;
                    }
                }
                else
                {
                    //无此任务则加入任务列表
                    QuestManager.Instance.tasks.Add(newTask);
                    //任务状态改为已开始（newTask为临时变量，直接更改不影响List内任务状态）
                    QuestManager.Instance.GetTask(newTask.questData).IsStarted = true;
                    
                    //接受任务时判断背包内是否有任务物品
                    foreach (var requireItem in newTask.questData.RequireTargetName())
                    {
                        InventoryManager.Instance.CheckQuestItemInBug(requireItem);
                    }
                }
            }
        }
        if (nextPieceID == "")
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            return;
        }
        else
        {
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currentData.dialogueIndex[nextPieceID]);
        }
    }

    //跟新option
    public void UpdateOption(DialoguePiece piece, DialogueOption option)
    {
        currentPiece = piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
        takeQuest = option.takeQuest;
    }
}
