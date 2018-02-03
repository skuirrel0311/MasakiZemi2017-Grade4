using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// アイテムがエリアに配置された時にイベントを発生させる
/// </summary>
public class EventAreaItemSaucer : BaseItemSaucer
{
    [Serializable]
    public struct MyEvent
    {
        public UnityEvent onSetItem;
        public string acceptObjectName;
        public string flagName;
    }

    [SerializeField]
    MyEvent[] events = null;

    public override void SetItem(HoloItem item, bool showParticle = true)
    {
        SetObject(showParticle, item);
    }

    public virtual void SetCharacter(HoloCharacter character, bool showParticle = true)
    {
        SetObject(showParticle, character);
    }

    void SetObject(bool showParticle, HoloObject obj)
    {
        for(int i = 0;i< events.Length;i++)
        {
            if (events[i].acceptObjectName != obj.GetName()) continue;

            if(events[i].onSetItem != null) events[i].onSetItem.Invoke();

            if(!string.IsNullOrEmpty(events[i].flagName))
            {
                FlagManager.I.SetFlag(events[i].flagName, null);
            }
        }

        if (showParticle) PlayParticle();

        base.SetItem(null);
    }

    void PlayParticle()
    {
        AkSoundEngine.PostEvent("Equip", gameObject);
        ParticleManager.I.Play("Doron", owner.transform.position, Quaternion.identity);
    }

    public override bool CheckCanHaveItem(HoloItem item)
    {
        return CheckCanHaveObject(item);
    }

    public bool CheckCanHaveItem(HoloCharacter character)
    {
        return CheckCanHaveObject(character);
    }

    protected virtual bool CheckCanHaveObject(HoloObject obj)
    {
        for (int i = 0; i < events.Length; i++)
        {
            if (events[i].acceptObjectName == obj.GetName()) return true;
        }

        return false;
    }
}
