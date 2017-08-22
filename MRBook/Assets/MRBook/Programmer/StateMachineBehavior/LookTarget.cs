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
    Transform actor;
    Animator m_animator;
    Quaternion to;
    int state = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_animator = animator;
        target = ActorManager.I.GetTargetPoint(targetName);
        actor = ActorManager.I.GetActor(actorName).transform;
        Vector3 targetDirection = target.position - actor.position;
        
        to = Quaternion.LookRotation(targetDirection);

        NavMeshAgent agent = actor.GetComponent<NavMeshAgent>();
        if(agent != null)
        {
            agent.updateRotation = false;
        }

        StateMachineManager.I.Add(actorName + "Look", new MyTask(OnUpdate, OnEnd));
    }

    void OnUpdate()
    {
        actor.rotation = Quaternion.RotateTowards(actor.rotation, to, rotationSpeed * Time.deltaTime);

        float angle = Quaternion.Angle(actor.rotation, to);
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
        NavMeshAgent agent = actor.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.updateRotation = true;
        }

        m_animator.SetInteger("LookTargetState", state);
    }
}
