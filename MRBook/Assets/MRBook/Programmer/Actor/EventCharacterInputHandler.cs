using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベントエリアに置くことでなにか起きるキャラクターのInputHandler
/// </summary>
public class EventCharacterInputHandler : HoloMovableObjInputHander
{
    HoloCharacter ownerCharacter;

    public override void Init(HoloObject owner)
    {
        ownerCharacter = (HoloCharacter)owner;

        base.Init(owner);
    }

    public override MakerType OnDragUpdate(HitObjType hitObjType, HoloObject hitObj)
    {
        if(hitObj == null || hitObj.ItemSaucer == null)
        {
            return base.OnDragUpdate(hitObjType, hitObj);
        }

        if(hitObjType == HitObjType.EventArea)
        {
            EventAreaItemSaucer eventArea = (EventAreaItemSaucer)hitObj.ItemSaucer;

            if (eventArea.CheckCanHaveItem(ownerCharacter)) return MakerType.Happen;
        }

        return MakerType.DontPut;
    }

    public override void OnDragEnd(HitObjType hitObjType, HoloObject hitObj)
    {
        if (hitObjType != HitObjType.EventArea)
        {
            base.OnDragEnd(hitObjType, hitObj);
            return;
        }

        EventAreaItemSaucer eventArea = (EventAreaItemSaucer)hitObj.ItemSaucer;

        if (eventArea.CheckCanHaveItem(ownerCharacter))
        {
            eventArea.SetCharacter(ownerCharacter);
        }
    }
}
