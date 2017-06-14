using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookTarget : StateMachineBehaviour
{
    public string targetName;
    public string actorName;
    public float rotationSpeed = 100.0f;
    protected Transform target;
    protected Transform actor;

    Quaternion to;

    bool isEnd = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target = ActorManager.I.GetAnchor(targetName);
        actor = ActorManager.I.GetActor(actorName).transform;
        Vector3 targetDirection = target.position - actor.position;
        
        to = Quaternion.LookRotation(targetDirection);

        NavMeshAgent agent = actor.GetComponent<NavMeshAgent>();
        if(agent != null)
        {
            agent.updateRotation = false;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isEnd) return;
        actor.rotation = Quaternion.RotateTowards(actor.rotation, to, rotationSpeed * Time.deltaTime);

        float angle = Quaternion.Angle(actor.rotation, to);
        if(angle < 0.5f)
        {
            Debug.Log("回転終わり");
            //回転し終わった
            animator.SetTrigger("IsCompleted");
            isEnd = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NavMeshAgent agent = actor.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.updateRotation = true;
        }
    }
}
