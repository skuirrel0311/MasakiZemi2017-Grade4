using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//～まで行けという命令
public class GoThere : StateMachineBehaviour
{
    public string actorName;
    public string targetName;
    public float stopDistance = 1.0f;

    GameObject actor;
    NavMeshAgent agent;

    bool isEnd = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        actor = ActorManager.I.GetActor(actorName).gameObject;
        Vector3 targetPosition = ActorManager.I.GetAnchor(targetName).position;
        agent = actor.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = false;
            agent.SetDestination(targetPosition);
            agent.stoppingDistance = stopDistance;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isEnd) return;
        if (agent == null)
        {
            Debug.Log("agent is null");
            isEnd = true;
            animator.SetTrigger("IsCompleted");
            return;
        }

        NavMeshPath path = agent.path;
        float distance = 0.0f;
        Vector3 temp = actor.transform.position;

        for(int i = 0;i< path.corners.Length;i++)
        {
            Vector3 conner = path.corners[i];
            distance += Vector3.Distance(temp, conner);
            temp = conner;
        }

        if(distance < stopDistance)
        {
            agent.isStopped = true;
            isEnd = true;
            animator.SetTrigger("IsCompleted");
        }
    }
}
