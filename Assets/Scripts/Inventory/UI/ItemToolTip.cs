using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    public Text itemNameText;
    public Text itemInfoText;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetupTooltip(ItemData_SO item)
    {
        itemNameText.text = item.itemName;
        itemInfoText.text = item.description;
    }

    private void OnEnable()
    {
        //初始化tooltip位置，防止抖动出现
        updatePosition();
    }

    private void Update()
    {
        updatePosition();
    }

    //实现tooltip随鼠标移动的效果
    public void updatePosition()
    {
        //获取当前toolTip的世界坐标
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        
        //获取tooltip的宽和高
        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;
        
        //获取鼠标位置
        Vector3 mousePos = Input.mousePosition;

        //设置普适性tooltip初始位置。轴心在左上角，据鼠标10*10像素
        rectTransform.position = mousePos + Vector3.down * 10 + Vector3.right * 10;
        
        //判断到鼠标过于靠下时，应调整
        if (mousePos.y - 10 < height)
        {
            rectTransform.position += Vector3.up * (height - (mousePos.y - 10));
        }
        //判断到鼠标过于靠右时，应调整
        if (Screen.width - (mousePos.x + 10) < width)
        {
            rectTransform.position += Vector3.left * (20 + width);
        }
    }
}
