using UnityEngine;

public class ChildEndPoint : BaseStateMachineBehaviour
{
    public override TaskType GetTaskType { get { return TaskType.EndPoint; } }

    public override void Init(int selfIndex, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.Init(selfIndex, animator, stateInfo, layerIndex);
        //強制的に成功
        CurrentStatus = BehaviourStatus.Success;
    }
}
