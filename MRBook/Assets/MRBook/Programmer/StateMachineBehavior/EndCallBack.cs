using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCallBack : BaseStateMachineBehaviour
{
    public bool success = false;

    public override void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStart(animator, stateInfo, layerIndex);
        MainSceneManager.I.EndCallBack(success);
    }
}
