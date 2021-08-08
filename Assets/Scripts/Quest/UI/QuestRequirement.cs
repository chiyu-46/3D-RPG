using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    private Text requireName;
    private Text progressNumber;

    private void Awake()
    {
        requireName = GetComponent<Text>();
        progressNumber = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetupRequirement(string name, int amount, int currentAmount)
    {
        requireName.text = name;
        progressNumber.text = currentAmount.ToString() +" / " + amount.ToString();
    }
    
    //任务完成后不应更新进度信息
    public void SetupRequirement(string name, bool isFinished)
    {
        if (isFinished)
        {
            requireName.text = name;
            progressNumber.text = "完成";
            requireName.color = Color.gray;
            progressNumber.color =Color.gray;
        }
    }
}
