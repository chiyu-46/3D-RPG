using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private Text levelText;
    private Image healthSlider;
    private Image expSlider;
    private CharacterStates playerStates;

    private void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        if (!playerStates)
        {
            playerStates = GameManager.Instance.playerStats;
        }
        levelText.text = "Level  " + playerStates.characterData.currentLevel.ToString("00");
        UpdateHealth();
        UpdateExp();
    }

    private void UpdateExp()
    {
        float sliderPercent = (float) playerStates.characterData.currentExp / playerStates.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }

    private void UpdateHealth()
    {
        float sliderPercent = (float) playerStates.CurrentHealth / playerStates.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }
}
