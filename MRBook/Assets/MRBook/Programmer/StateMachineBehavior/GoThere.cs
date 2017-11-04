using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//～まで行けという命令
public class GoThere : StateMachineBehaviour
{
    public ActorName actorName;
    public string targetName;
    public float stopDistance = 0.2f;
    public float moveSpeed = 0.1f;

    HoloCharacter character;
    int state = 0;

    float time;
    const float limitTime = 1.0f;
    Vector3 oldPosition;

    Animator m_animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character = ActorManager.I.GetCharacter(actorName);
        if(character == null)
        {
            Debug.LogError(actorName.ToString() + " is null");
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

        character.m_agent.speed = moveSpeed;
        m_animator = animator;
        m_animator.SetInteger("GoThereState", 0);

        character.m_agent.isStopped = false;
        character.m_agent.SetDestination(target.position);
        character.m_agent.stoppingDistance = stopDistance;
        oldPosition = character.transform.position;

        string animationName = MotionNameManager.GetMotionName(MotionName.Walk, character);

        character.m_animator.CrossFade(animationName, 0.1f);
        StateMachineManager.I.Add(actorName.ToString() + "Go",new MyTask(OnUpdate, OnEnd));
    }

    void OnUpdate()
    {
        NavMeshPath path = character.m_agent.path;
        float distance = 0.0f;
        Vector3 temp = character.transform.position;
        
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
            StateMachineManager.I.Stop(actorName.ToString() + "Go");
        }

        //動けなくなった
        Vector3 movement = oldPosition - character.transform.position;
        if (movement.magnitude < 0.0001f)
        {
            time += Time.deltaTime;
            if (time > limitTime)
            {
                state = -1;
                StateMachineManager.I.Stop(actorName.ToString() + "Go");
            }
        }

        oldPosition = character.transform.position;
    }

    void OnEnd()
    {
        character.m_agent.isStopped = true;
        string animationName = MotionNameManager.GetMotionName(MotionName.Wait, character);
        character.m_animator.CrossFade(animationName, 0.1f);
        m_animator.SetInteger("GoThereState", state);
    }
    //中断
    void Suspension()
    {
        state = -1;
        OnEnd();
    }
}
