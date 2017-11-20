using System.Collections.Generic;
using UnityEngine;

public class ParentTask : BaseStateMachineBehaviour
{
    protected List<BaseStateMachineBehaviour> childTask = new List<BaseStateMachineBehaviour>();

    public override void Init(int selfIndex, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.Init(selfIndex, animator, stateInfo, layerIndex);

        SetChildTask();
    }

    protected virtual void SetChildTask() { }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        childTask.Clear();
    }
}
