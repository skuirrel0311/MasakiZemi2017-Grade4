using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//～まで行けという命令
public class GoThere : StateMachineBehaviour
{
    public string actorName;
    public string targetName;
    public float stopDistance = 0.2f;

    GameObject actor;
    NavMeshAgent agent;
    int state = 0;

    Animator m_animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        actor = ActorManager.I.GetActor(actorName).gameObject;
        Vector3 targetPosition = ActorManager.I.GetTargetPoint(targetName).position;
        agent = actor.GetComponent<NavMeshAgent>();
        m_animator = animator;
        m_animator.SetInteger("GoThereState", 0);

        if (agent == null)
        {
            Debug.LogError("agent is null");
            state = -1;
            OnEnd();
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(targetPosition);
        agent.stoppingDistance = stopDistance;
        StateMachineManager.I.Add(actorName,new MyTask(OnUpdate, OnEnd));
    }

    void OnUpdate()
    {
        Debug.Log("call on update");
        NavMeshPath path = agent.path;
        float distance = 0.0f;
        Vector3 temp = actor.transform.position;

        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 conner = path.corners[i];
            distance += Vector3.Distance(temp, conner);
            temp = conner;
        }

        //たどり着いた
        if (distance < stopDistance)
        {
            state = 1;
            StateMachineManager.I.Stop(actorName);
        }
    }

    void OnEnd()
    {
        Debug.Log("set go there " + state);
        agent.isStopped = true;
        m_animator.SetInteger("GoThereState", state);
    }
}
