using System.Collections;
using UnityEngine;

public class UrashimaGo : MoveObject
{
    HoloPuppet puppet;

    [SerializeField]
    float rotationSpeed = 200.0f;
    Quaternion to;

    bool isRotating = false;

    [SerializeField]
    bool callChangeAnimation = true;

    protected override void OnStart()
    {
        puppet = (HoloPuppet)ActorManager.I.GetObject(objectName);
        base.OnStart();
        
        StateMachineManager.I.StartCoroutine(MonitorPuppet());
    }

    protected override void SetTargetPoint(int index)
    {
        base.SetTargetPoint(index);

        //目的地が設定されたので回転を始める
        if(callChangeAnimation) puppet.ChangeAnimationClip(MotionName.Walk, 0.1f);
        to = GetTargetDirectionRot(wayPoints[currentIndex]);
    }


    IEnumerator MonitorPuppet()
    {
        while (true)
        {
            if (puppet.Puppet.urashimaState == RootMotion.Dynamics.PuppetMaster.UrashimaState.Dead) break;
            yield return null;
        }

        CurrentStatus = BehaviourStatus.Failure;
        OnEnd();
    }

    protected override void OnEnd()
    {
        if(callChangeAnimation) puppet.ChangeAnimationClip(MotionName.Wait, 0.1f);
        isActive = false;
        StateMachineManager.I.StopCoroutine(MonitorPuppet());
        if (!hasRootTask) m_animator.SetInteger("StateStatus", CurrentStatus == BehaviourStatus.Success ? 1 : -1);
    }

    protected override BehaviourStatus OnUpdate()
    {
        if(isRotating)
        {
            if(IsJustLook())
            {
                isRotating = false;
            }
            return BehaviourStatus.Running;
        }

        return base.OnUpdate();
    }

    Quaternion GetTargetDirectionRot(Transform target)
    {
        return Quaternion.LookRotation(target.position - puppet.transform.position);
    }

    protected bool IsJustLook()
    {
        puppet.transform.rotation = Quaternion.RotateTowards(puppet.transform.rotation, to, rotationSpeed * Time.deltaTime);

        float angle = Quaternion.Angle(puppet.transform.rotation, to);
        return angle < 0.5f;
    }
}
