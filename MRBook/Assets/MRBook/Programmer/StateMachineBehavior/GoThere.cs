using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//～まで行けという命令
public class GoThere : StateMachineBehaviour
{
    public string actorName;
    public string targetName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject actor = ActorManager.I.GetActor(actorName);
        Vector3 targetPosition = ActorManager.I.GetTarget(targetName).position;
        NavMeshAgent nav = actor.GetComponent<NavMeshAgent>();
        if (nav != null) nav.SetDestination(targetPosition);
    }
}
