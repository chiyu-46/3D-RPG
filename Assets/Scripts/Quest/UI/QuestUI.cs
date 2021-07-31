using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
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
}
