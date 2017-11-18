using System.Collections.Generic;

public class ParentTask : BaseStateMachineBehaviour
{
    [UnityEngine.SerializeField]
    protected List<BaseStateMachineBehaviour> childTask = new List<BaseStateMachineBehaviour>();

    public override void Init(int selfIndex)
    {
        base.Init(selfIndex);

        SetChildTask();
    }

    protected virtual void SetChildTask() { }
}
