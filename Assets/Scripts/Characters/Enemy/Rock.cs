using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class Rock : MonoBehaviour
{
    public enum RockStates { HitPlayer, HitEnemy, HitNothing }
    private Rigidbody rb;
    private Vector3 diection;
    [HideInInspector]
    public RockStates rockStates;

    [Header("Basic Settings")] 
    public float force;
    public GameObject target;
    public int damage;
    public GameObject breakEffect;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        rockStates = RockStates.HitPlayer;
        FlyToTarget();
    }

    void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1f)
        {
            rockStates = RockStates.HitNothing;
        }  
    }
    private void FlyToTarget()
    {
        if (!target)
        {
            target = FindObjectOfType<PlayerController>().gameObject;
        }
        diection = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(diection * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (rockStates)
        {
            case RockStates.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = diection * force;
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterStates>().TakeDamage(damage,other.gameObject.GetComponent<CharacterStates>());
                    rockStates = RockStates.HitNothing;
                }
                break;
            case RockStates.HitEnemy:
                if (other.gameObject.GetComponent<Golem>())
                {
                    var otherStates = other.gameObject.GetComponent<CharacterStates>();
                    otherStates.TakeDamage(damage,otherStates);
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
        }
    }
}
