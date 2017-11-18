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
    //瞬間判定か継続判定か？
    public bool checkOnUpdate = false;

    protected override void OnStart()
    {
        base.OnStart();
        m_animator.SetBool("Flagged", FlagManager.I.GetFlag(flagName, isCheckNow) == boolValue);

        if(checkOnUpdate)
        {
            StateMachineManager.I.Add(flagName, new MyTask(OnUpdate1));
        }
    }

    public void OnUpdate1()
    {
        m_animator.SetBool("Flagged", FlagManager.I.GetFlag(flagName, true) == boolValue);
    }
}
