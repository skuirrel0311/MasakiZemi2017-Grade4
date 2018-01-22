﻿using UnityEngine;

public class Parallel : Composite
{
    protected override void OnStart()
    {
        base.OnStart();
        for (int i = 0; i < childTask.Count; i++)
        {
            //全部起動
            childTask[i].Start();
        }
    }

    protected override BehaviourStatus OnUpdate()
    {
        if (IsEndChildTask())
        {
            CurrentStatus = BehaviourStatus.Success;
            for (int i = 0; i < childTask.Count; i++)
            {
                if (childTask[i].CurrentStatus == BehaviourStatus.Failure)
                {
                    CurrentStatus = BehaviourStatus.Failure;
                }
            }
            return CurrentStatus;
        }

        return BehaviourStatus.Running;
    }

    protected bool IsEndChildTask()
    {
        for (int i = 0; i < childTask.Count; i++)
        {
            //一つでもisEndがfalseだったら終わっていない
            if (!childTask[i].IsEnd) return false;
        }
        return true;
    }

    protected override void OnEnd()
    {
        isActive = false;

        Debug.Log("parallel = " + CurrentStatus.ToString());

        int state = CurrentStatus == BehaviourStatus.Success ? 1 : -1;
        if (!hasRootTask) m_animator.SetInteger("StateStatus", state);
    }
}
