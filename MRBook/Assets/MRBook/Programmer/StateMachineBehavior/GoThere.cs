using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//～まで行けという命令
public class GoThere : BaseStateMachineBehaviour
{
    [SerializeField]
    public ActorName actorName;

    public enum TargetType { StaticPoint, HoloObject }
    [SerializeField]
    TargetType targetType = TargetType.StaticPoint;
    [SerializeField]
    public string targetName;

    [SerializeField]
    bool updateTargetPosition = false;
    Transform target;

    [SerializeField]
    public float stopDistance = 0.2f;
    [SerializeField]
    public float moveSpeed = 0.1f;
    
    protected HoloCharacter character;
    protected int state = 0;

    float time;
    const float limitTime = 1.0f;
    Vector3 oldPosition;
    
    protected override void OnStart()
    {
        base.OnStart();
        character = ActorManager.I.GetCharacter(actorName);
        if(character == null)
        {
            Debug.LogError(actorName.ToString() + " is null in" + actorName.ToString() + " go there");
            Suspension();
            return;
        }
        target = ActorManager.I.GetTargetPoint(targetName);
        if(target == null)
        {
            Debug.LogError(targetName + " is null" + " is null in" + actorName.ToString() + " go there");
            Suspension();
            return;
        }

        character.m_agent.speed = moveSpeed;
        character.m_agent.isStopped = false;
        character.m_agent.SetDestination(target.position);
        character.m_agent.stoppingDistance = stopDistance;
        oldPosition = character.transform.position;

        string animationName = MotionNameManager.GetMotionName(MotionName.Walk, character);

        character.m_animator.CrossFade(animationName, 0.1f);
    }

    protected void SetTarget()
    {
        switch (targetType)
        {
            case TargetType.StaticPoint:
                target = ActorManager.I.GetTargetPoint(targetName);
                break;
            case TargetType.HoloObject:
                HoloObject obj = ActorManager.I.GetObject(targetName);
                if (obj == null) break;
                target = obj.transform;

                break;
        }

        if (target == null)
        {
            Debug.LogError(targetName + " is null");
            Suspension();
            return;
        }
    }

    protected override BehaviourStatus OnUpdate()
    {
        if (IsJustHere())
        {
            Debug.Log(actorName.ToString() + "到着");
            return BehaviourStatus.Success;
        }
        if (IsDeadAgent())
        {
            Debug.Log(actorName.ToString() + "移動失敗");
            return BehaviourStatus.Failure;
        }

        if(updateTargetPosition)
        {
            character.m_agent.SetDestination(target.position);
        }
        return BehaviourStatus.Running;
    }

    protected bool IsJustHere()
    {
        if (!character.m_agent.hasPath) return false;

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
        return distance < stopDistance;
    }

    protected bool IsDeadAgent()
    {
        //動けなくなった
        Vector3 movement = oldPosition - character.transform.position;
        if (movement.magnitude < 0.0001f)
        {
            time += Time.deltaTime;
            if (time > limitTime)
            {
                return true;
            }
        }
        oldPosition = character.transform.position;
        return false;
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        StopAgent();
    }

    protected virtual void StopAgent()
    {
        if (character == null || ActorManager.I.GetTargetPoint(targetName) == null) return;
        character.m_agent.isStopped = true;
        string animationName = MotionNameManager.GetMotionName(MotionName.Wait, character);
        character.m_animator.CrossFade(animationName, 0.1f);

    }
    //中断
    void Suspension()
    {
        CurrentStatus = BehaviourStatus.Failure;
        OnEnd();
    }
}
