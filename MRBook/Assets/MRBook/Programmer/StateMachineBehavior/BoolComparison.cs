using UnityEngine;

/// <summary>
/// 指定されたフラグの値とboolValueの値が等しいかをメカニムのFlaggedに入れます
/// </summary>
public class BoolComparison : BaseStateMachineBehaviour
{
    public string flagName;
    public bool boolValue = true;
    //現在の値を使うか再生が開始されたタイミングの値を使うか？
    public bool isCheckNow = true;

    //継続的にチェックする場合はフラグがTrueにならないとタスクが終了しないので注意
    //瞬間判定か継続判定か？
    public bool checkOnUpdate = false;

    [SerializeField]
    string paramName = "Flagged";

    protected override void OnStart()
    {
        base.OnStart();
        m_animator.SetBool(paramName, FlagManager.I.GetFlag(flagName, isCheckNow) == boolValue);

        if(checkOnUpdate)
        {
            StateMachineManager.I.Add(flagName, new MyTask(UpdateFlagged));
        }
    }

    BehaviourStatus UpdateFlagged()
    {
        bool flagged = FlagManager.I.GetFlag(flagName, isCheckNow) == boolValue;

        if (flagged) return BehaviourStatus.Success;
        
        return BehaviourStatus.Running;
    }
}
