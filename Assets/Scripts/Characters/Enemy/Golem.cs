using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Golem : EnemyController
{
    [Header("Skill")] 
    public float kickForce = 25;
    public GameObject rockPrefab;
    public Transform handPos;
    
    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();
            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            targetStates.GetComponent<NavMeshAgent>().isStopped = true;
            targetStates.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
            targetStates.TakeDamage(characterStates, targetStates);
        }
    }

    public void ThrowRock()
    {
        var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
        rock.GetComponent<Rock>().target = attackTarget;
    }
}
