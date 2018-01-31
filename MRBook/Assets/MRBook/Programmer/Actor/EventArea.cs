using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventArea : BaseItemSaucer
{
    [SerializeField]
    string acceptObjectName = "";

    [SerializeField]
    string animatorStateName = "";

    public override void SetItem(HoloItem item, bool showParticle = true)
    {
        if (showParticle)
        {
            AkSoundEngine.PostEvent("Equip", gameObject);
            ParticleManager.I.Play("Doron", owner.transform.position, Quaternion.identity);
        }

        MainSceneManager.I.m_Animator.CrossFade(animatorStateName, 0.0f);
    }

    public override bool CheckCanHaveItem(HoloItem item)
    {
        if (item.GetName() == acceptObjectName) return true;
        return false;
    }
}
