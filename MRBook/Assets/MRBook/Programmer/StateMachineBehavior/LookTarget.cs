using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookTarget : StateMachineBehaviour
{
    public string targetName;
    public string actorName;
    public float rotationSpeed = 100.0f;

    Transform target;
    HoloMovableObject actor;
    Animator m_animator;
    Quaternion to;
    int state = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_animator = animator;
        actor = ActorManager.I.GetActor(actorName);
        if (actor == null)
        {
            Debug.LogError(actorName + " is null");
            Suspension();
            return;
        }
        Transform target = ActorManager.I.GetTargetPoint(targetName);
        if (target == null)
        {
            Debug.LogError(targetName + " is null");
            Suspension();
            return;
        }
        
        Vector3 targetDirection = target.position - actor.transform.position;
        
        to = Quaternion.LookRotation(targetDirection);
        actor.m_agent.updateRotation = false;
        actor.m_animator.CrossFade("Walk", 0.1f);

        StateMachineManager.I.Add(actorName + "Look", new MyTask(OnUpdate, OnEnd));
    }

    void OnUpdate()
    {
        actor.transform.rotation = Quaternion.RotateTowards(actor.transform.rotation, to, rotationSpeed * Time.deltaTime);

        float angle = Quaternion.Angle(actor.transform.rotation, to);
        if(angle < 0.5f)
        {
            Debug.Log("回転終わり");
            //回転し終わった
            state = 1;
            StateMachineManager.I.Stop(actorName + "Look");
        }
    }

    void OnEnd()
    {
        actor.m_agent.updateRotation = true;
        actor.m_animator.CrossFade("Wait", 0.1f);
        m_animator.SetInteger("LookTargetState", state);
    }

    //中断
    void Suspension()
    {
        state = -1;
        OnEnd();
    }
}
