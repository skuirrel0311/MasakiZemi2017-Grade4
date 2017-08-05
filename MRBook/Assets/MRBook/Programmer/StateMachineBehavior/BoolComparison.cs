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
    public bool isCheckNow = true;
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
