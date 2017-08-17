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

    float time;
    const float limitTime = 1.0f;
    Vector3 oldPosition;

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
        oldPosition = actor.transform.position;
        StateMachineManager.I.Add(actorName + "Go",new MyTask(OnUpdate, OnEnd));
    }

    void OnUpdate()
    {
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
            Debug.Log("移動終了");
            StateMachineManager.I.Stop(actorName + "Go");
        }

        Vector3 movement = oldPosition - actor.transform.position;
        Debug.Log("movement.mag = " + movement.magnitude);
        if (movement.magnitude < 0.0001f)
        {
            time += Time.deltaTime;
            if (time > limitTime)
            {
                state = -1;
                StateMachineManager.I.Stop(actorName + "Go");
            }
        }

        oldPosition = actor.transform.position;
    }

    void OnEnd()
    {
        agent.isStopped = true;
        m_animator.SetInteger("GoThereState", state);
    }
}
