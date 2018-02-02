using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// アイテムがエリアに配置された時にイベントを発生させる
/// </summary>
public class EventAreaItemSaucer : BaseItemSaucer
{
    [SerializeField]
    UnityEvent onSetItem = null;

    [SerializeField]
    string acceptObjectName = "";

    public override void SetItem(HoloItem item, bool showParticle = true)
    {
        SetObject(showParticle);
    }

    public virtual void SetCharacter(HoloCharacter character, bool showParticle = true)
    {
        SetObject(showParticle);
    }

    void SetObject(bool showParticle)
    {
        if (onSetItem != null) onSetItem.Invoke();
        if (showParticle)
        {
            AkSoundEngine.PostEvent("Equip", gameObject);
            ParticleManager.I.Play("Doron", owner.transform.position, Quaternion.identity);
        }
    }

    public override bool CheckCanHaveItem(HoloItem item)
    {
        return item.GetName() == acceptObjectName;
    }

    public bool CheckCanHaveItem(HoloCharacter character)
    {
        return character.GetName() == acceptObjectName;
    }
}
