using UnityEngine;

public class GoThereInBackground : GoThere
{
    const string paramName = "GoThereState";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("call on state enter in go there");
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (m_animator == null) Debug.Log("animator is null");
        //ここが呼ばれたらほんとナゾ
        if (animator == null) Debug.Log("???");
    }

    protected override void OnStart()
    {
        base.OnStart();
        m_animator.SetInteger(paramName, 0);
        StateMachineManager.I.Add(actorName.ToString() + "Go", new MyTask(base.OnUpdate, EndTask));
    }

    protected override BehaviourStatus OnUpdate()
    {
        return BehaviourStatus.Success;
    }

    void EndTask()
    {
        m_animator.SetInteger(paramName, CurrentStatus == BehaviourStatus.Success ? 1 : -1);
        base.StopAgent();
    }

    protected override void StopAgent() { }
}
