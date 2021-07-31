using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elements")] 
    public Image icon;
    public Text mainText;
    public Button nextButton;
    public GameObject dialoguePanel;
    
    [Header("Options")]
    public RectTransform optionPanel;
    public OptionUI optionPrefab;

    [Header("Data")] 
    public DialogueDataSO currentData;          //当前的对话数据。此值由DialogueController决定。
    private int currentIndex;                   //List dialoguePieces的序号

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    //点击next时执行
    private void ContinueDialogue()
    {
        if (currentIndex < currentData.dialoguePieces.Count)
        {
            UpdateMainDialogue(currentData.dialoguePieces[currentIndex]);
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }

    //确定当前要显示的对话数据，并确保从第一条开始显示。
    public void UpdateDialogueData(DialogueDataSO data)
    {
        currentData = data;
        currentIndex = 0;
    }
    
    //显示并更新对话UI
    public void UpdateMainDialogue(DialoguePiece piece)
    {
        currentIndex++;
        dialoguePanel.SetActive(true);
        //显示图片
        if (piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else
        {
            icon.enabled = false;
        }

        //显示对话文本
        mainText.text = "";
        mainText.DOText(piece.text, 1f);

        if (piece.options.Count == 0 && currentData.dialoguePieces.Count > 0)
        {
            nextButton.interactable = true;
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            nextButton.interactable = false;
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        
        //创建option
        CreationOptions(piece);
    }

    private void CreationOptions(DialoguePiece piece)
    {
        //如果当前存在选项，则清除现有选项。用于初始化和更新option
        if (optionPanel.childCount > 0)
        {
            for (int i = 0; i < optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }
        //创建新选项
        for (int i = 0; i < piece.options.Count; i++)
        {
            var option = Instantiate(optionPrefab, optionPanel);
            option.UpdateOption(piece, piece.options[i]);
        }
    }
}
