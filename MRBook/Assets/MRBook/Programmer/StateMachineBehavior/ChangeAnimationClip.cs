using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimationClip : StateMachineBehaviour
{
    public ActorName actorName;
    public MotionName motionName;
    public MotionType[] types = { MotionType.Normal };
    public float transitionDuration = 0.1f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HoloMovableObject actor = ActorManager.I.GetActor(actorName.ToString());
        
        if(actor == null)
        {
            Debug.LogError(actorName + "is not found actor");
            return;
        }

        string animationName = MotionNameManager.GetMotionName(motionName, actor);
        
        actor.m_animator.CrossFade(animationName, transitionDuration);
    }
}
