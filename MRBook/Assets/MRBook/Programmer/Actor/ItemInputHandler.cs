using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInputHandler : HoloMovableObjInputHander
{
    HoloItem ownerItem;

    public override void Init(HoloObject owner)
    {
        ownerItem = (HoloItem)owner;

        base.Init(owner);
    }

    public override bool OnClick()
    {
        //アイテムの説明を表示
        SetItemTextEnable(true);
        base.OnClick();
        return true;
    }

    public override void OnDragStart()
    {
        //アイテムの説明を非表示
        SetItemTextEnable(false);
        base.OnDragStart();
    }

    public override MakerType OnDragUpdate(HitObjType hitObjType, HoloObject hitObj)
    {
        if (hitObj == null || hitObj.ItemSaucer == null)
        {
            return base.OnDragUpdate(hitObjType, hitObj);
        }

        if (hitObj.CheckCanHaveItem(ownerItem)) return MakerType.Happen;
        else return MakerType.DontPut;
    }

    public override void OnDragEnd(HitObjType hitObjType, HoloObject hitObj)
    {
        if (hitObj == null || hitObj.ItemSaucer == null)
        {
            base.OnDragEnd(hitObjType, hitObj);
            return;
        }

        if (hitObj.CheckCanHaveItem(ownerItem))
        {
            hitObj.SetItem(ownerItem);
            return;
        }

        //Itemが持てるのにここまで来たってことは持たせることができなかったということ
    }

    public void SetItemTextEnable(bool enabled)
    {

    }
}
