using UnityEngine;

/// <summary>
/// シーケンス
/// </summary>
public class Sequence : Composite
{
    int currentTaskIndex;

    protected override void OnStart()
    {
        base.OnStart();
        currentTaskIndex = 0;
        childTask[currentTaskIndex].Start();
    }

    protected override BehaviourStatus OnUpdate()
    {
        if (childTask[currentTaskIndex].CurrentStatus != BehaviourStatus.Running)
        {
            //todo:子タスクが失敗した場合も継続でいいのか？
            if (!StartNextTask()) return BehaviourStatus.Success;
        }

        return BehaviourStatus.Running;
    }

    /// <summary>
    /// 次のタスクを開始する
    /// </summary>
    bool StartNextTask()
    {
        currentTaskIndex++;
        if (currentTaskIndex >= childTask.Count)
        {
            //範囲外
            return false;
        }

        childTask[currentTaskIndex].Start();
        return true;
    }

    protected override void OnEnd()
    {
        base.OnEnd();
    }
}
