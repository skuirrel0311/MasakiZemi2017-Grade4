using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : EventAreaItemSaucer
{
    [SerializeField]
    string parentName = "";
    [SerializeField]
    MotionName motionName = MotionName.Wait;

    public override void SetCharacter(HoloCharacter character, bool showParticle = true)
    {
        HoloObject parent = ActorManager.I.GetObject(parentName);

        character.transform.parent = parent.transform;
        character.transform.localPosition = Vector3.zero;
        character.ChangeAnimationClip(motionName, 0.0f);
        
        MainSceneManager.I.Play();
    }
}
