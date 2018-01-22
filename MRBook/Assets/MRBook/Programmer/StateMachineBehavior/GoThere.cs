using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//～まで行けという命令
public class GoThere : BaseStateMachineBehaviour
{
    [Serializable]
    struct Target
    {
        public ActorManager.TargetType targetType;
        public string targetName;
    }

    [SerializeField]
    public ActorName actorName;
    [SerializeField]
    ActorManager.TargetType targetType = ActorManager.TargetType.StaticPoint;
    [SerializeField]
    public string targetName;

    [SerializeField]
    bool updateTargetPosition = false;
    Transform target;

    [SerializeField, Range(0.02f, 1.0f)]
    public float stopDistance = 0.05f;
    [SerializeField]
    public float moveSpeed = 0.1f;

    protected HoloCharacter character;
    protected int state = 0;

    float time;
    const float limitTime = 1.0f;
    Vector3 oldPosition;

    float updateTimer = 0.0f;

    float navMeshUpdateTimer = 0.0f;

    [SerializeField]
    Target[] targets = null;
    Transform[] wayPoints = null;

    int currentIndex;
    Transform currentTarget;

    protected override void OnStart()
    {
        base.OnStart();
        Debug.Log(actorName + " Go " + targetName);

        character = ActorManager.I.GetCharacter(actorName);
        if (character == null)
        {
            Debug.LogError(actorName.ToString() + " is not found in " + actorName.ToString() + " go there");
            Suspension();
            return;
        }

        MyNavMeshBuilder.CreateNavMesh();
        updateTimer = 0.0f;
        time = 0.0f;
        character.m_agent.enabled = true;
        character.m_agent.speed = moveSpeed;
        character.m_agent.isStopped = false;
        character.m_agent.stoppingDistance = stopDistance;
        oldPosition = character.transform.position;

        character.ChangeAnimationClip(MotionName.Walk, 0.1f);
        
        if (targets != null && targets.Length > 0)
        {
            wayPoints = new Transform[targets.Length];

            for (int i = 0; i < wayPoints.Length; i++)
            {
                wayPoints[i] = ActorManager.I.GetTargetTransform(targets[i].targetName, targets[i].targetType);
            }

            SetTarget(currentIndex);
            return;
        }

        //targetsに入力がなかった（複数ではない）
        target = ActorManager.I.GetTargetTransform(targetName, targetType);
        if (target != null)
        {
            currentTarget = target;
            character.m_agent.SetDestination(target.position);
        }
    }

    void SetTarget(int index)
    {
        if (wayPoints[index] == null) return;
        currentTarget = wayPoints[index];
        character.m_agent.SetDestination(wayPoints[index].position);
    }

    protected override BehaviourStatus OnUpdate()
    {
        UpdateNavMesh();

        if (IsJustHere())
        {
            if (wayPoints == null) return BehaviourStatus.Success;

            currentIndex++;
            if (currentIndex >= wayPoints.Length)
            {
                Debug.Log(actorName.ToString() + "到着");
                return BehaviourStatus.Success;
            }

            SetTarget(currentIndex);

            return BehaviourStatus.Running;
        }
        if (IsDeadAgent())
        {
            Debug.Log(actorName.ToString() + "移動失敗");
            return BehaviourStatus.Failure;
        }

        if (updateTargetPosition)
        {
            updateTimer += Time.deltaTime;
            if (updateTimer > 0.2f)
            {
                updateTimer = 0.0f;
                character.m_agent.SetDestination(currentTarget.position);
            }
        }
        return BehaviourStatus.Running;
    }

    void UpdateNavMesh()
    {
        navMeshUpdateTimer += Time.deltaTime;
        MyNavMeshBuilder.CreateNavMesh();
        if (navMeshUpdateTimer > 0.2f)
        {

            navMeshUpdateTimer = 0.0f;
        }
    }

    protected bool IsJustHere()
    {
        if (currentTarget == null) return false;
        float distance = Vector3.Distance(character.transform.position, currentTarget.position);

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
        Debug.Log("call stop agent");
        if (character == null || currentTarget == null) return;
        character.m_agent.isStopped = true;
        character.m_agent.ResetPath();
        character.m_agent.enabled = false;
        character.ChangeAnimationClip(MotionName.Wait, 0.1f);

    }
    //中断
    void Suspension()
    {
        CurrentStatus = BehaviourStatus.Failure;
        OnEnd();
    }
}
