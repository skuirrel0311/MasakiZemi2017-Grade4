using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//～まで行けという命令
public class GoThere : StateMachineBehaviour
{
    public string actorName;
    public string targetName;

    GameObject actor;
    NavMeshAgent agent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        actor = ActorManager.I.GetActor(actorName);
        Vector3 targetPosition = ActorManager.I.GetTarget(targetName).position;
        agent = actor.GetComponent<NavMeshAgent>();
        if (agent != null) agent.SetDestination(targetPosition);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null)
        {
            Debug.Log("agent is null");
            animator.SetBool("IsMoved", true);
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

        if(distance < 0.5f)
        {
            animator.SetBool("IsMoved", true);
        }
    }
}
