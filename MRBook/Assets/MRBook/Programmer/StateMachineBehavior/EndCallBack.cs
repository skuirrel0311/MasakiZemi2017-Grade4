using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCallBack : StateMachineBehaviour
{
    public bool success = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (success)
        {
            //todo:次のページへ
        }
        else
        {
            //もう一度再生できるようにする。
            #if UNITY_EDITOR
                animator.SetBool("IsStart", false);
            #else
                MainGameManager.I.EndCallBack();
            #endif
        }
    }
}
