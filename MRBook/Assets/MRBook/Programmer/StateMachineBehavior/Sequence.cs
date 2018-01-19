using UnityEngine;

/// <summary>
/// シーケンス
/// </summary>
public class Sequence : Composite
{
    int currentTaskIndex;

    //子タスクが失敗しても次のタスクを実行するか？
    [SerializeField]
    bool isForcing = true;

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
            //Debug.Log(childTask[currentTaskIndex].ToString() + " status =  " + childTask[currentTaskIndex].CurrentStatus.ToString());
            //todo:子タスクが失敗した場合も継続でいいのか？
            if(!isForcing && childTask[currentTaskIndex].CurrentStatus == BehaviourStatus.Failure)
            {
                return BehaviourStatus.Failure;
            }
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
        isActive = false;
        int state = CurrentStatus == BehaviourStatus.Success ? 1 : -1;
        if (!hasRootTask) m_animator.SetInteger("StateStatus", state);
    }
}
