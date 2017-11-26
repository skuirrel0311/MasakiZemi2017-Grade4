using UnityEngine;

public class LookTarget : BaseStateMachineBehaviour
{
    public enum TargetType { StaticPoint, HoloObject }
    [SerializeField]
    TargetType targetType = TargetType.StaticPoint;
    public string targetName;
    public ActorName actorName;
    public float rotationSpeed = 100.0f;

    HoloCharacter character;
    Quaternion to;

    [SerializeField]
    bool updateTargetPosition = false;
    Transform target;

    protected override void OnStart()
    {
        base.OnStart();
        character = ActorManager.I.GetCharacter(actorName);
        if (character == null)
        {
            Debug.LogError(actorName + " is null");
            Suspension();
            return;
        }

        SetTarget();

        Vector3 targetDirection = target.position - character.transform.position;

        to = Quaternion.LookRotation(targetDirection);
        character.m_agent.updateRotation = false;

        character.ChangeAnimationClip(MotionName.Walk, 0.1f);
    }

    void SetTarget()
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

    Quaternion GetTargetDirectionRot()
    {
        return Quaternion.LookRotation(target.position - character.transform.position);
    }

    protected override BehaviourStatus OnUpdate()
    {
        if (IsJustLook()) return BehaviourStatus.Success;

        if (updateTargetPosition)
        {
            to = GetTargetDirectionRot();
        }

        return BehaviourStatus.Running;
    }

    protected bool IsJustLook()
    {
        character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, to, rotationSpeed * Time.deltaTime);

        float angle = Quaternion.Angle(character.transform.rotation, to);
        return angle < 0.5f;
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        character.m_agent.updateRotation = true;
        character.ChangeAnimationClip(MotionName.Walk, 0.1f);
    }

    //中断
    void Suspension()
    {
        CurrentStatus = BehaviourStatus.Failure;
        OnEnd();
    }
}
