using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyStates { GUARD, PATROL, CHASE, DEAD }

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStates))]

public class EnemyController : MonoBehaviour, IEndGameObserver
{
    private NavMeshAgent agent;
    private EnemyStates enemyStates;
    protected GameObject attackTarget;
    protected CharacterStates characterStates;
    private float lastAttackTime;
    private float speed;
    private Animator anim;
    private bool isWalk;
    private bool isChase;
    private bool isFollow;
    private Vector3 wayPoint;
    private Vector3 guardPos;
    private Quaternion guardRotation;
    private bool isDead;
    private Collider coll;
    private bool isPlayerDead;
        
    [Header("Base Settings")] 
    public bool isGuard;
    public float sightRadius;
    public float lookAtTime;
    private float remainLookAtTime;

    [Header("Patrol State")] 
    public float patrolRange;
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        anim = GetComponent<Animator>();
        remainLookAtTime = lookAtTime;
        characterStates = GetComponent<CharacterStates>();
        lastAttackTime = 0;
        coll = GetComponent<Collider>();
        isPlayerDead = false;
    }

    private void Update()
    {
        if (characterStates.CurrentHealth == 0)
        {
            isDead = true;
        }

        if (!isPlayerDead)
        {
           SwichStates();
           SwitchAnimation(); 
        }
        
    }

    private void Start()
    {
        guardPos = transform.position;
        guardRotation = transform.rotation;
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
        //FIXME:场景切换后修改
        GameManager.Instance.AddObserver(this);
    }

    // private void OnEnable()
    // {
    //     GameManager.Instance.AddObserver(this);
    // }

    private void OnDisable()
    {
        if (!GameManager.IsInitialized) return;
        GameManager.Instance.RemoveObserver(this);
        //死亡掉落物品
        if (GetComponent<LootSpawner>() && isDead)
        {
            GetComponent<LootSpawner>().SpawnLoot();
        }
    }

    void SwichStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;
            isChase = false;
        }
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                isChase = false;
                if (transform.position != guardPos)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;
                    if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
                    {
                        isWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.1f);
                    }
                }
                
                break;
            case EnemyStates.PATROL:
                isChase = false;
                agent.speed = speed * 0.5f;
                //是否到了随机巡逻点
                if (Vector3.Distance(wayPoint,transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime > 0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else
                    {
                        GetNewWayPoint();
                        remainLookAtTime = lookAtTime;
                    }
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
                break;
            case EnemyStates.CHASE:
                isWalk = false;
                isChase = true;
                agent.speed = speed;
                if (!FoundPlayer())
                {
                    isFollow = false;
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (isGuard)
                    {
                        enemyStates = EnemyStates.GUARD;
                    }
                    else
                    {
                        enemyStates = EnemyStates.PATROL;
                    }
                }
                else
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }

                if (TargetInAttackRange() || TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;
                    if (lastAttackTime <= 0)
                    {
                        lastAttackTime = characterStates.coolDown;
                        //暴击判断
                        characterStates.isCritical = Random.value < characterStates.criticalChance;
                        //执行攻击
                        Attack();
                    }
                    else
                    {
                        lastAttackTime -= Time.deltaTime;
                    }
                }
                
                break;
            case EnemyStates.DEAD:
                coll.enabled = false;
                agent.radius = 0;
                Destroy(gameObject,2f);
                break;
        }
    }

    private void Attack()
    {
        transform.LookAt(attackTarget.transform);
        if (TargetInAttackRange())
        {
            //近战
            anim.SetTrigger("Attack");
        }
        else
        {
            //远程
            anim.SetTrigger("Skill");
        }
    }

    void SwitchAnimation()
    {
        anim.SetBool("Walk",isWalk);
        anim.SetBool("Chase",isChase);
        anim.SetBool("Follow",isFollow);
        anim.SetBool("Critical", characterStates.isCritical);
        anim.SetBool("Death", isDead);
    }
    
    bool FoundPlayer()
    {
        Collider[] colliders = new Collider[1];
        int count = Physics.OverlapSphereNonAlloc(transform.position, sightRadius, colliders, 1 << 3);
        if (count > 0)
        {
            attackTarget = colliders[0].gameObject;
            return true;
        }

        attackTarget = null;
        return false;
    }

    bool TargetInAttackRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStates.attackRange;
        }
        else
        {
            return false;
        }
    }

    bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStates.skillRange;
        }
        else
        {
            return false;
        }
    }
    
    void GetNewWayPoint()
    {
        float randomX = Random.Range(-patrolRange,patrolRange);
        float randomZ = Random.Range(-patrolRange,patrolRange);
        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y,guardPos.z + randomZ);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;

        wayPoint = new Vector3(guardPos.x + randomX, transform.position.y,guardPos.z + randomZ);
    }

    void Hit()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();
            targetStates.TakeDamage(characterStates, targetStates);
        }
    }

    public void EndNotify()
    {
        // 敌人获得玩家死亡广播，播放获胜动画，停止移动，停止agent
        isPlayerDead = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;
        anim.SetBool("Win",true);
    }
}
