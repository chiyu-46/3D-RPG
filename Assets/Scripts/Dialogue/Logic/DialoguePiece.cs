using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePiece
{
    public string ID;       //用于对话序号
    public Sprite image;    //npc头像
    [TextArea]
    public string text;     //对话文本

    public List<DialogueOption> options = new List<DialogueOption>();
}
