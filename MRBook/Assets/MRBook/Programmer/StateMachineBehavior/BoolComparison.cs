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

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Flagged", FlagManager.I.GetFlag(flagName) == boolValue);
    }
}
