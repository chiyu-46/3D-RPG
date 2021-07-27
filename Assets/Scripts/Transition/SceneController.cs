using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>, IEndGameObserver
{
    private GameObject player;
    public GameObject playerPrefab;
    public SceneFade sceneFadePrefab;
    private NavMeshAgent playerAgent;
    private bool fadeFinished;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GameManager.Instance.AddObserver(this);
        fadeFinished = true;
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationType));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationType));
                break;
        }
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationType destinationType)
    {
        SaveManager.Instance.SavePlayerData();
        InventoryManager.Instance.SaveData();
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            TransitionDestination transitionDestination = GetDestination(destinationType);
            yield return Instantiate(playerPrefab, transitionDestination.transform.position,transitionDestination.transform.rotation);
            SaveManager.Instance.LoadPlayerData();
            yield break;
        }
        else
        {
            player = GameManager.Instance.playerStats.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;
            TransitionDestination transitionDestination = GetDestination(destinationType);
            player.transform.SetPositionAndRotation(transitionDestination.transform.position, transitionDestination.transform.rotation);
            playerAgent.enabled = true;
            yield return null;
        }
        
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationType destinationType)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        foreach (var entrance in entrances)
        {
            if (entrance.destinationType == destinationType)
            {
                return entrance;
            }
        }
        return null;
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("Game"));
    }
    
    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }
    
    IEnumerator LoadLevel(string scene)
    {
        SceneFade fade = Instantiate(sceneFadePrefab);
        if (scene != "")
        {
            yield return StartCoroutine(fade.FadeOut(2.5f));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab,GameManager.Instance.GetEntrance().position,
                GameManager.Instance.GetEntrance().rotation);
            //保存数据
            SaveManager.Instance.SavePlayerData();
            yield return StartCoroutine(fade.FadeIn(2.5f));
            yield break;
        }
    }

    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }
    
    IEnumerator LoadMain()
    {
        SceneFade fade = Instantiate(sceneFadePrefab);
        yield return StartCoroutine(fade.FadeOut(2.5f));
        yield return SceneManager.LoadSceneAsync("Main");
        yield return StartCoroutine(fade.FadeIn(2.5f));
        yield break;
    }

    public void EndNotify()
    {
        if (fadeFinished)
        {
            fadeFinished = false;
            StartCoroutine(LoadMain());
        }
    }
}
