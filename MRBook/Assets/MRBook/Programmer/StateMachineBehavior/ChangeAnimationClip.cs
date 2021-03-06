﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimationClip : BaseStateMachineBehaviour
{
    public ActorName actorName;
    public MotionName motionName;
    public float transitionDuration = 0.1f;
    public string WwiseEventName = string.Empty;

    protected override void OnStart()
    {
        base.OnStart();
        HoloCharacter actor = ActorManager.I.GetCharacter(actorName);
        
        if(actor == null)
        {
            Debug.LogError(actorName + "is not found actor in change animation clip");
            return;
        }

        actor.ChangeAnimationClip(motionName, transitionDuration);
        
        if(!string.IsNullOrEmpty(WwiseEventName))
        {
            AkSoundEngine.PostEvent(WwiseEventName, actor.gameObject);
        }
    }
}
