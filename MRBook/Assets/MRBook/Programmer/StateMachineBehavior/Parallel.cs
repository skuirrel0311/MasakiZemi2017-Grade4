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
}
