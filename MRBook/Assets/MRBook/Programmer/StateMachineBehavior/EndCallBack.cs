using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCallBack : StateMachineBehaviour
{
    public bool success = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            MainGameManager.I.EndCallBack(success);
    }
}
