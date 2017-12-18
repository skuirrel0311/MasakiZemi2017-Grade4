using UnityEngine;

public class LookTarget : BaseStateMachineBehaviour
{
    [SerializeField]
    ActorManager.TargetType targetType = ActorManager.TargetType.StaticPoint;
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

        target = ActorManager.I.GetTargetTransform(targetName, targetType);
        
        if (target == null)
        {
            Debug.LogError(targetName + " is null");
            Suspension();
            return;
        }

        Vector3 targetDirection = target.position - character.transform.position;

        to = Quaternion.LookRotation(targetDirection);
        if(character.m_agent != null) character.m_agent.updateRotation = false;

        character.ChangeAnimationClip(MotionName.Walk, 0.1f);
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
        if(character.m_agent != null) character.m_agent.updateRotation = true;
        character.ChangeAnimationClip(MotionName.Wait, 0.1f);
    }

    //中断
    void Suspension()
    {
        CurrentStatus = BehaviourStatus.Failure;
        OnEnd();
    }
}
