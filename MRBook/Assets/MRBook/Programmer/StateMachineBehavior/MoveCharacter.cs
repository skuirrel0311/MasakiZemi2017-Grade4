using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MoveObject
{
    [SerializeField]
    float rotationSpeed = 200.0f;
    [SerializeField]
    bool callChangeAnimation = true;

    bool isRotating = false;
    Quaternion to;
    HoloCharacter character;

    protected override void OnStart()
    {
        character = (HoloCharacter)ActorManager.I.GetObject(objectName);
        Debug.Log("start " + character.name + " Move");
        base.OnStart();
    }

    protected override void SetTargetPoint(int index)
    {
        base.SetTargetPoint(index);

        //目的地が設定されたので回転を始める
        isRotating = true;
        if (callChangeAnimation) character.ChangeAnimationClip(MotionName.Walk, 0.1f);
        to = GetTargetDirectionRot(wayPoints[currentIndex]);
    }

    protected override void OnEnd()
    {
        Debug.Log(character.name + " MoveEnd");
        if (callChangeAnimation) character.ChangeAnimationClip(MotionName.Wait, 0.1f);
        isActive = false;
        if (!hasRootTask) m_animator.SetInteger("StateStatus", CurrentStatus == BehaviourStatus.Success ? 1 : -1);
    }

    protected override BehaviourStatus OnUpdate()
    {
        if (isRotating)
        {
            if (IsJustLook())
            {
                isRotating = false;
            }
            return BehaviourStatus.Running;
        }

        return base.OnUpdate();
    }

    Quaternion GetTargetDirectionRot(Transform target)
    {
        return Quaternion.LookRotation(target.position - character.transform.position);
    }

    protected bool IsJustLook()
    {
        character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, to, rotationSpeed * Time.deltaTime);

        float angle = Quaternion.Angle(character.transform.rotation, to);
        
        return angle < 0.5f;
    }
}
