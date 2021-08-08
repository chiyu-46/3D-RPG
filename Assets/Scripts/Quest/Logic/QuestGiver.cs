using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    private DialogueController controller;
    private QuestData_SO currentQuest;
    
    public DialogueDataSO startDialogue;
    public DialogueDataSO progressDialogue;
    public DialogueDataSO completeDialogue;
    public DialogueDataSO finishDialogue;

    private void Awake()
    {
        controller = GetComponent<DialogueController>();
    }

    private void Start()
    {
        controller.currentData = startDialogue;
        currentQuest = controller.currentData.GetQuest();
    }

    private void Update()
    {
        if (IsStart)
        {
            if (IsComplete)
            {
                controller.currentData = completeDialogue;
            }
            else
            {
                controller.currentData = progressDialogue;
            }
        }

        if (IsFinished)
        {
            controller.currentData = finishDialogue;
        }
    }

    #region 获取任务状态

    public bool IsStart
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).IsStarted;
            }
            return false;
        }
    }
    
    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).IsComplete;
            }
            return false;
        }
    }
    
    public bool IsFinished
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).IsFinished;
            }
            return false;
        }
    }

    #endregion
}
