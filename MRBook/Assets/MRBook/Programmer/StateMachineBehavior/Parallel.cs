using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Composite
{
    bool childrenEnd = false;

    public override void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStart(animator, stateInfo, layerIndex);
        Debug.Log("on parallel start");
        for(int i = 0;i < childTask.Count;i++)
        {
            childTask[i].OnTaskEnd += () =>{
                childrenEnd = IsAllTaskEndInChild();
            };

            //全部起動
            childTask[i].isActive = true;
            childTask[i].OnStart(animator, stateInfo, layerIndex);
        }
    }

    public override BehaviourStatus OnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (childrenEnd) return BehaviourStatus.Success;

        return BehaviourStatus.Running;
    }

    bool IsAllTaskEndInChild()
    {
        //後ろから見たほうが効率が良い
        for (int i = childTask.Count - 1; i > 0; i--)
        {
            //一つでもisEndがfalseだったら終わっていない
            if (!childTask[i].isEnd) return false;
        }
        return true;
    }

    public override void OnExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("on parallel end");
        base.OnExit(animator, stateInfo, layerIndex);
    }
}
