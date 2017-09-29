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
    public float moveSpeed = 0.1f;

    HoloMovableObject actor;
    int state = 0;

    float time;
    const float limitTime = 1.0f;
    Vector3 oldPosition;

    Animator m_animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        actor = ActorManager.I.GetActor(actorName);
        if(actor == null)
        {
            Debug.LogError(actorName + " is null");
            Suspension();
            return;
        }
        Transform target = ActorManager.I.GetTargetPoint(targetName);
        if(target == null)
        {
            Debug.LogError(targetName + " is null");
            Suspension();
            return;
        }

        actor.m_agent.speed = moveSpeed;
        m_animator = animator;
        m_animator.SetInteger("GoThereState", 0);

        actor.m_agent.isStopped = false;
        actor.m_agent.SetDestination(target.position);
        actor.m_agent.stoppingDistance = stopDistance;
        oldPosition = actor.transform.position;
        actor.m_animator.CrossFade("Walk", 0.1f);
        StateMachineManager.I.Add(actorName + "Go",new MyTask(OnUpdate, OnEnd));
    }

    void OnUpdate()
    {
        NavMeshPath path = actor.m_agent.path;
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

        //動けなくなった
        Vector3 movement = oldPosition - actor.transform.position;
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
        actor.m_agent.isStopped = true;
        actor.m_animator.CrossFade("Wait", 0.1f);
        m_animator.SetInteger("GoThereState", state);
    }
    //中断
    void Suspension()
    {
        state = -1;
        OnEnd();
    }
}
