using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string text;     //选项文本
    public string targetID; //下一条对话ID
    public bool takeQuest;  //是否接受任务
}
