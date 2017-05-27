using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCallBack : StateMachineBehaviour
{
    public bool isEditor = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isEditor) MainGameManager.I.EndCallBack();
        else TestSceneManager.I.EndCallBack();
    }
}
