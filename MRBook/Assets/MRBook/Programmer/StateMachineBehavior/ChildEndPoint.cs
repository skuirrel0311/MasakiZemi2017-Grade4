
public class ChildEndPoint : BaseStateMachineBehaviour
{
    public override TaskType GetTaskType { get { return TaskType.EndPoint; } }

    public override void Init(int selfIndex)
    {
        base.Init(selfIndex);
        //強制的に成功
        CurrentStatus = BehaviourStatus.Success;
    }
}
