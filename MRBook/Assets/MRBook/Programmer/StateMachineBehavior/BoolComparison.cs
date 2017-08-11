using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定されたフラグの値とboolValueの値が等しいかをメカニムのFlaggedに入れます
/// </summary>
public class BoolComparison : StateMachineBehaviour
{
    public string flagName;
    public bool boolValue = true;
    //現在の値を使うか再生が開始されたタイミングの値を使うか？
    public bool isCheckNow = true;
    //瞬間判定か継続判定か？
    public bool isOnUpdate = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Flagged", FlagManager.I.GetFlag(flagName, isCheckNow) == boolValue);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //todo:常にフラグをチェックする。
    }
}
