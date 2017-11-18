using UnityEngine;

public class LookTarget : BaseStateMachineBehaviour
{
    public string targetName;
    public ActorName actorName;
    public float rotationSpeed = 100.0f;
    
    HoloCharacter character;
    Quaternion to;
    int state = 0;

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

        StateMachineManager.I.Add(actorName.ToString() + "Look", new MyTask(OnUpdate1, OnEnd1));
    }

    void OnUpdate1()
    {
        character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, to, rotationSpeed * Time.deltaTime);

        float angle = Quaternion.Angle(character.transform.rotation, to);
        if(angle < 0.5f)
        {
            Debug.Log("回転終わり");
            //回転し終わった
            state = 1;
            StateMachineManager.I.Stop(actorName + "Look");
        }
    }

    void OnEnd1()
    {
        character.m_agent.updateRotation = true;
        string animationName = MotionNameManager.GetMotionName(MotionName.Wait, character);
        character.m_animator.CrossFade(animationName, 0.1f);
        m_animator.SetInteger("LookTargetState", state);
    }

    //中断
    void Suspension()
    {
        state = -1;
        OnEnd();
    }
}
