using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyActor : StateMachineBehaviour
{
    public string actorName;
    //何秒後に消すか
    public float duration = 0.0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(ActorManager.I.GetActor(actorName),duration);
    }
}
