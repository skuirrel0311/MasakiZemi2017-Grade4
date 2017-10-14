using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : StateMachineBehaviour
{
    public string objectName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ActorManager.I.DisableActor(objectName);
    }
}
