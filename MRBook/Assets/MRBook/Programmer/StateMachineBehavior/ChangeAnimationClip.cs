using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimationClip : StateMachineBehaviour
{
    public string actorName;
    public string animationName;
    public float transitionDuration = 0.1f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HoloObject actor = ActorManager.I.GetActor(actorName);
        
        if(actor == null)
        {
            Debug.LogError(actorName + "is not found actor in change animation clip");
            return;
        }

        Animator actorAnimator = actor.GetComponent<Animator>();

        if (actorAnimator == null)
        {
            Debug.LogError(actorName + " don't attach animator");
            return;
        }

        actorAnimator.CrossFade(animationName, transitionDuration);
    }
}
