using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : BaseStateMachineBehaviour
{
    public string objectName;

    public override void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStart(animator, stateInfo, layerIndex);
        ActorManager.I.DisableObject(objectName);
    }
}
