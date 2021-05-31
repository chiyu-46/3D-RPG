using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button newGameBtn;
    private Button ContinueBtn;
    private Button quitBtn;

    private PlayableDirector director;
    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        ContinueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();
        
        newGameBtn.onClick.AddListener(PlayTimeline);
        ContinueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);

        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
    }

    private void PlayTimeline()
    {
        director.Play();
    }

    private void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.TransitionToFirstLevel();
    }
    
    private void ContinueGame()
    {
        SceneController.Instance.TransitionToLoadGame();
    }
    
    void QuitGame()
    {
        Application.Quit();
    }
}
