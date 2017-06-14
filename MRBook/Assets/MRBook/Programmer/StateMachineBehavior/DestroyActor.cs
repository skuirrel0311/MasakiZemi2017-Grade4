using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyActor : StateMachineBehaviour
{
    public string actorName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ActorManager.I.DisableActor(actorName);
    }
}
