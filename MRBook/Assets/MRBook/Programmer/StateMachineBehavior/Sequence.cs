using UnityEngine;

/// <summary>
/// シーケンス
/// </summary>
public class Sequence : Composite
{
    int currentTaskIndex = 0;

    protected override void OnStart()
    {
        base.OnStart();
        childTask[currentTaskIndex].Start();
    }

    protected override BehaviourStatus OnUpdate()
    {
        Debug.Log("on update child num is " + childTask.Count);
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
}
