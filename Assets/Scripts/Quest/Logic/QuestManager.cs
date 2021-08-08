using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [System.Serializable]
    public class QuestTask
    {
        public QuestData_SO questData;
        public bool IsStarted
        {
            get { return questData.isStarted; }
            set { questData.isStarted = value; }
        }
        public bool IsComplete
        {
            get { return questData.isComplete; }
            set { questData.isComplete = value; }
        }
        public bool IsFinished
        {
            get { return questData.isFinished; }
            set { questData.isFinished = value; }
        }
    }

    public List<QuestTask> tasks = new List<QuestTask>();

    private void Start()
    {
        LoadQuestManager();
    }

    public void LoadQuestManager()
    {
        var questCount = PlayerPrefs.GetInt("QuestCount");
        for (int i = 0; i < questCount; i++)
        {
            var newQuest = ScriptableObject.CreateInstance<QuestData_SO>();
            SaveManager.Instance.Load(newQuest,"task" + i);
            tasks.Add(new QuestTask{questData = newQuest});
        }
    }

    public void SaveQuestManager()
    {
        PlayerPrefs.SetInt("QuestCount",tasks.Count);
        for (int i = 0; i < tasks.Count; i++)
        {
            SaveManager.Instance.Save(tasks[i].questData,"task" + i);
        }
    }
    
    //敌人死亡或拾取物品时调用
    public void UpdateQuestProgress(string requireName, int amount)
    {
        foreach (var take in tasks)
        {
            if (take.IsFinished)
            {
                continue;
            }
            var matchTask = take.questData.questRequires.Find(r => r.name == requireName);
            if (matchTask != null)
            {
                matchTask.currentAmount += amount;
            }
            //更新进度的同时，判断任务是否完成
            take.questData.CheckQuestProgress();
        }
    }
    public bool HaveQuest(QuestData_SO data)
    {
        if (data != null)
        {
            return tasks.Any(q => q.questData.questName == data.questName);
        }
        else
        {
            return false;
        }
    }

    public QuestTask GetTask(QuestData_SO data)
    {
        return tasks.Find(q => q.questData.questName == data.questName);
    }
}
