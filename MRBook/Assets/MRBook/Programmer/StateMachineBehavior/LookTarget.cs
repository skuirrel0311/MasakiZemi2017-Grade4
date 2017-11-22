using UnityEngine;

public class LookTarget : BaseStateMachineBehaviour
{
    public string targetName;
    public ActorName actorName;
    public float rotationSpeed = 100.0f;
    
    HoloCharacter character;
    Quaternion to;

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
        Transform target = ActorManager.I.GetTargetPoint(targetName);
        if (target == null)
        {
            Debug.LogError(targetName + " is null");
            Suspension();
            return;
        }
        
        Vector3 targetDirection = target.position - character.transform.position;
        
        to = Quaternion.LookRotation(targetDirection);
        character.m_agent.updateRotation = false;

        string animationName = MotionNameManager.GetMotionName(MotionName.Walk, character);

        character.m_animator.CrossFade(animationName, 0.1f);
    }

    protected override BehaviourStatus OnUpdate()
    {
        if (IsJustLook()) return BehaviourStatus.Success;

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
