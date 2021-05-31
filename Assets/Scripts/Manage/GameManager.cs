using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public CharacterStates playerStats;

    public List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();
    private CinemachineFreeLook followCamera;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    
    public void RigisterPlayer(CharacterStates player)
    {
        playerStats = player;
        followCamera = FindObjectOfType<CinemachineFreeLook>();
        if (followCamera != null)
        {
            followCamera.Follow = playerStats.transform.GetChild(3);
            followCamera.LookAt = playerStats.transform.GetChild(3);
        }
    }

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }

    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationType == TransitionDestination.DestinationType.Enter)
            {
                return item.transform;
            }
        }

        return null;
    }
}
