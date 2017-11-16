using System;
using UnityEngine;

public class BaseStateMachineBehaviour : StateMachineBehaviour
{
    public enum BehaviourStatus { Running, Success, Failure }
    /// <summary>
    /// 更新タイミングはアクティブ時にベースのUpdateが呼ばれたタイミング
    /// </summary>
    public BehaviourStatus CurrentStatus { get; protected set; }
    /// <summary>
    /// Compositによって制御されている場合はOnStateEnterが呼ばれる前にfalseになる
    /// </summary>
    [NonSerialized]
    public bool isActive = true;

    [NonSerialized]
    public bool isEnd = false;

    [NonSerialized]
    public Action OnTaskEnd = null;

    [NonSerialized]
    public bool isInitialize = false;

    public virtual bool HasChild { get { return false; } }
    public enum TaskType { Action, Composite, Decrators , EndPoint}
    public virtual TaskType GetTaskType { get{return TaskType.Action; } }
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isActive) return;
        if (isInitialize) return;

        //ここの処理が呼ばれるということはCompositによって制御されていないということ

        //Compositeの代わりにイベントを呼び出す
        OnStart(animator, stateInfo, layerIndex);
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isActive) return;

        if (CurrentStatus != BehaviourStatus.Running) return;

        CurrentStatus = OnUpdate(animator, stateInfo, layerIndex);
        if (CurrentStatus !=  BehaviourStatus.Running) OnExit(animator, stateInfo, layerIndex);
    }

    /// <summary>
    /// ステートの開始時（Compositeの有無によって呼ばれるタイミングが異なる）
    /// </summary>
    public virtual void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isInitialize = true;
    }
    
    /// <summary>
    /// ステートがActiveの時に呼ばれる
    /// </summary>
    public virtual BehaviourStatus OnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { return BehaviourStatus.Running; }

    /// <summary>
    /// ステートの終了時に呼ばれる
    /// </summary>
    public virtual void OnExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isEnd = true;
        if (OnTaskEnd != null) OnTaskEnd.Invoke();
        RootTask.I.OnEndTask();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isEnd = false;
        isActive = true;
        isInitialize = false;
        OnTaskEnd = null;
    }

    /// <summary>
    /// 子タスクを格納する
    /// </summary>
    /// <param name="selfIndex">自身のインデックス</param>
    public virtual void InitChildTask(int selfIndex) { }
}
