using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimationClip : BaseStateMachineBehaviour
{
    public ActorName actorName;
    public MotionName motionName;
    public float transitionDuration = 0.1f;
    public string WwiseEventName = string.Empty;

    public override void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStart(animator, stateInfo, layerIndex);
        HoloCharacter actor = ActorManager.I.GetCharacter(actorName);
        
        if(actor == null)
        {
            Debug.LogError(actorName + "is not found actor");
            return;
        }

        string animationName = MotionNameManager.GetMotionName(motionName, actor);
        
        actor.m_animator.CrossFade(animationName, transitionDuration);
        if(!string.IsNullOrEmpty(WwiseEventName))
        {
            AkSoundEngine.PostEvent(WwiseEventName, actor.gameObject);
        }
    }
}
