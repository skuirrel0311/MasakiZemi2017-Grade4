using UnityEngine;

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
        if (IsEndChildTask()) return BehaviourStatus.Success;

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
        
        for(int i = 0;i< childTask.Count;i++)
        {
            if(childTask[i].CurrentStatus == BehaviourStatus.Failure)
            {
                CurrentStatus = BehaviourStatus.Failure;
            }
        }

        int state = CurrentStatus == BehaviourStatus.Success ? 1 : -1;
        if (!hasRootTask) m_animator.SetInteger("StateStatus", state);
    }
}
