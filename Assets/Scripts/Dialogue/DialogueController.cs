using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueDataSO currentData;
    private bool canTalk = false;

    //这两个方法用于当player进出触发范围时控制canTalk变量变化。
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentData != null)
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = false;
            DialogueUI.Instance.dialoguePanel.SetActive(false);
        }
    }

    private void Update()
    {
        //按下鼠标右键将打开对话UI面板
        if (canTalk && Input.GetMouseButtonDown(1))
        {
            OpenDialogue();
        }
    }

    //打开对话UI并确定对话内容
    private void OpenDialogue()
    {
        //打开UI面板
        //传输对话内容
        DialogueUI.Instance.UpdateDialogueData(currentData);
        DialogueUI.Instance.UpdateMainDialogue(currentData.dialoguePieces[0]);
    }
}
