using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    private Animator anim;

    private GameObject attackTarget;

    private CharacterStates characterStates;
    private Collider coll;
    private bool isDead;
    private float stopDistance;
    private float lastAttactTime;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
        characterStates = GetComponent<CharacterStates>();
        coll = GetComponent<Collider>();
        stopDistance = agent.stoppingDistance;
    }

    private void OnEnable()
    {
        GameManager.Instance.RigisterPlayer(characterStates);
        StartCoroutine(FindMouseManager());
    }

    IEnumerator FindMouseManager()
    {
        while (MouseManager.Instance == null)
        {
            yield return null;
        }
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
    }
    private void Start()
    {
        lastAttactTime = characterStates.coolDown;
        SaveManager.Instance.LoadPlayerData();
    }

    private void OnDisable()
    {
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
    }

    private void Update()
    {
        isDead = characterStates.CurrentHealth == 0;
        if (isDead)
        {
            GameManager.Instance.NotifyObservers();
        }
        SwitchAnimation();
        lastAttactTime -= Time.deltaTime;
    }

    private void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if (isDead) return;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;
    }
    
    private void SwitchAnimation()
    {
        anim.SetFloat("Speed",agent.velocity.sqrMagnitude);
        anim.SetBool("Death", isDead);
    }
    public void EventAttack(GameObject target)
    {
        if (isDead) return;
        if (target != null)
        {
            attackTarget = target;
            characterStates.isCritical = UnityEngine.Random.value < characterStates.criticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }
    
    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStates.attackData.attackRange;
        transform.LookAt(attackTarget.transform);
        while (Vector3.Distance(attackTarget.transform.position,transform.position) > characterStates.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;

        if (lastAttactTime < 0)
        {
            anim.SetBool("Critical",characterStates.isCritical);
            anim.SetTrigger("Attack");
            lastAttactTime = characterStates.coolDown;
        }
    }

    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            if (attackTarget.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().rockStates == Rock.RockStates.HitNothing)
            {
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20,ForceMode.Impulse);
            }
        }
        else
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();
            if (targetStates != null)
            {
                targetStates.TakeDamage(characterStates, targetStates);
            }
        }
        
    }
    
}
