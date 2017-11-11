using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーケンス
/// </summary>
public class Sequence : Composite
{
    int currentTaskIndex = 0;

    public override void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStart(animator, stateInfo, layerIndex);
        Debug.Log("on sequance start");

        for(int i = 0;i< childTask.Count;i++)
        {
            childTask[i].isActive = false;
        }

        if (childTask[currentTaskIndex] != null)
        {
            childTask[currentTaskIndex].isActive = true;
            childTask[currentTaskIndex].OnStart(animator, stateInfo, layerIndex);
        }
    }

    public override BehaviourStatus OnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(childTask[currentTaskIndex].CurrentStatus != BehaviourStatus.Running)
        {
            if (!StartNextTask(animator, stateInfo, layerIndex)) return BehaviourStatus.Success;
        }

        return BehaviourStatus.Running;
    }

    /// <summary>
    /// 次のタスクを開始する
    /// </summary>
    bool StartNextTask(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("call start next task");
        currentTaskIndex++;
        if(currentTaskIndex >= childTask.Count)
        {
            //範囲外
            return false;
        }

        childTask[currentTaskIndex - 1].isActive = false;
        childTask[currentTaskIndex].isActive = true;
        childTask[currentTaskIndex].OnStart(animator, stateInfo, layerIndex);
        return true;
    }

    public override void OnExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("on sequance end");
        base.OnExit(animator, stateInfo, layerIndex);
    }
}
