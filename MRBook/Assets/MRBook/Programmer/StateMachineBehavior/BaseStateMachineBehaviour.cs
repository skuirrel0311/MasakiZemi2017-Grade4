using System;
using UnityEngine;

public class BaseStateMachineBehaviour : StateMachineBehaviour
{
    public enum TaskType { Action, Composite, Decrators, EndPoint }
    public enum BehaviourStatus { Wait, Running, Success, Failure }

    /* メンバ */
    [NonSerialized]
    public bool isActive = true;

    protected int selfIndex = 0;
    protected StateMachineBehaviour parent = null;

    /* プロパティ */
    public virtual bool HasChild { get; protected set; }
    public virtual TaskType GetTaskType { get { return TaskType.Action; } }
    public BehaviourStatus CurrentStatus { get; protected set; }
    public bool IsEnd { get { return CurrentStatus != BehaviourStatus.Wait && CurrentStatus != BehaviourStatus.Running; } }
    public bool hasRootTask { get; protected set; }

    protected Animator m_animator { get; private set; }
    protected AnimatorStateInfo m_stateInfo { get; private set; }
    protected int m_layerIndex { get; private set; }

    /// <summary>
    /// 初期化(この関数を呼ぶのはRootだけ)
    /// </summary>
    public virtual void Init(int selfIndex)
    {
        this.selfIndex = selfIndex;
        isActive = false;
        hasRootTask = true;
    }

    public void SetParent(BaseStateMachineBehaviour parent)
    {
        this.parent = parent;
    }

    public void Start()
    {
        OnStart();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_animator = animator;
        m_stateInfo = stateInfo;
        m_layerIndex = layerIndex;

        if (hasRootTask) return;

        m_animator.SetInteger("StateStatus", 0);
        OnStart();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isActive) return;

        if (OnUpdate() != BehaviourStatus.Running) OnEnd();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //変数の初期化をしないとやばいかも？
        isActive = true;
    }

    protected virtual void OnStart()
    {
        //これをtrueにし忘れるとUpdateが動かない
        isActive = true;
        CurrentStatus = BehaviourStatus.Running;
    }

    protected virtual BehaviourStatus OnUpdate() { return BehaviourStatus.Success; }

    protected virtual void OnEnd()
    {
        isActive = false;
        if (!hasRootTask) m_animator.SetInteger("StateStatus", 1);
    }
}
