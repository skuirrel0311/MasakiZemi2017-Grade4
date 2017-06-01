using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCallBack : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
#if UNITY_EDITOR
#else
        MainGameManager.I.EndCallBack();
#endif
    }
}
