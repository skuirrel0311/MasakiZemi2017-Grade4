using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractHoloObjInputHandler
{
    public enum HitObjType { None, Book, Character, OtherObj }
    public enum MakerType { None, Normal, DontPut, PresentItem, DontPresentItem }
    public abstract void OnClick();
    public abstract void OnDragStart();
    public abstract MakerType OnDragUpdate(HitObjType hitObjType, HoloObject hitObj);
    public abstract void OnDragEnd(HitObjType hitObjType, HoloObject hitObj);
}

public class MovableObjInputHandler : AbstractHoloObjInputHandler
{
    protected HoloObject owner;
    AbstractInputHandlerBehaviour inputHandlerBehaviour;

    public MovableObjInputHandler(HoloObject owner)
    {
        this.owner = owner;
        if (owner.IsFloating) inputHandlerBehaviour = new FloatingObjDragEndBehaviour(owner);
        else inputHandlerBehaviour = new GroundingObjDragEndBehaviour(owner);
    }

    public override void OnClick()
    {
        MyObjControllerByBoundingBox.I.SetTargetObject(owner.gameObject);
    }

    public override void OnDragStart()
    {
        inputHandlerBehaviour.OnDragStart();
    }

    public override MakerType OnDragUpdate(HitObjType hitObjType, HoloObject hitObj)
    {
        return inputHandlerBehaviour.OnDragUpdate(hitObjType);
    }

    public override void OnDragEnd(HitObjType hitObjType, HoloObject hitObj)
    {
        inputHandlerBehaviour.OnDragEnd(hitObjType);
    }
}

public class ItemInputHandler : MovableObjInputHandler
{
    HoloItem ownerItem;

    public ItemInputHandler(HoloObject owner)
        : base(owner)
    {
        ownerItem = (HoloItem)owner;
    }

    public override void OnClick()
    {
        //アイテムの説明を表示
        SetItemTextEnable(true);
    }

    public override void OnDragStart()
    {
        //アイテムの説明を非表示
        SetItemTextEnable(false);
        base.OnDragStart();
    }

    public override MakerType OnDragUpdate(HitObjType hitObjType, HoloObject hitObj)
    {
        if(hitObjType == HitObjType.Character)
        {
            //持たせられるか、持たせられないかを判断する
            return MakerType.PresentItem;
        }
        return base.OnDragUpdate(hitObjType, hitObj);
    }

    public override void OnDragEnd(HitObjType hitObjType, HoloObject hitObj)
    {
        if(hitObjType == HitObjType.Character)
        {
            //キャラクターにアイテムを持たせる
            return;
        }

        base.OnDragEnd(hitObjType, hitObj);
    }

    public void SetItemTextEnable(bool enabled)
    {

    }
}


public abstract class AbstractInputHandlerBehaviour
{
    protected HoloObject owner;
    public AbstractInputHandlerBehaviour(HoloObject owner)
    {
        this.owner = owner;
    }
    public abstract void OnDragStart();
    public abstract AbstractHoloObjInputHandler.MakerType OnDragUpdate(AbstractHoloObjInputHandler.HitObjType hitObjType);
    public abstract void OnDragEnd(AbstractHoloObjInputHandler.HitObjType hitObjType);
}

public class GroundingObjDragEndBehaviour : AbstractInputHandlerBehaviour
{
    HoloMovableObject ownerMovableObj;
    Coroutine fallCoroutine;

    public override void OnDragStart()
    {
        ownerMovableObj.m_agent.enabled = false;
    }
    public override AbstractHoloObjInputHandler.MakerType OnDragUpdate(AbstractHoloObjInputHandler.HitObjType hitObjType)
    {
        switch(hitObjType)
        {
            case AbstractHoloObjInputHandler.HitObjType.None:
                return AbstractHoloObjInputHandler.MakerType.None;
            case AbstractHoloObjInputHandler.HitObjType.Book:
                return AbstractHoloObjInputHandler.MakerType.Normal;
            case AbstractHoloObjInputHandler.HitObjType.OtherObj:
            case AbstractHoloObjInputHandler.HitObjType.Character:
                return AbstractHoloObjInputHandler.MakerType.DontPut;
        }

        return AbstractHoloObjInputHandler.MakerType.None;
    }
    public GroundingObjDragEndBehaviour(HoloObject owner)
        : base(owner)
    {
        ownerMovableObj = (HoloMovableObject)owner;
    }
    public override void OnDragEnd(AbstractHoloObjInputHandler.HitObjType hitObjType)
    {
        if (hitObjType != AbstractHoloObjInputHandler.HitObjType.Book) return;

        if (fallCoroutine != null)
        {
            owner.StopCoroutine(fallCoroutine);
        }
        fallCoroutine = owner.StartCoroutine(Falling());
    }

    //0.1ずつ落ちて地面が近かったらAgentを有効化する
    IEnumerator Falling()
    {
        NavMeshHit hit;
        while (true)
        {
            //0.1ずつ下を探す
            owner.transform.position += Vector3.down * 0.1f;

            //todo : 絵本よりも下に行った場合はやばいのでなにか対応が必要

            if (NavMesh.SamplePosition(owner.transform.position, out hit, ownerMovableObj.m_agent.height, NavMesh.AllAreas))
            {
                break;
            }

            yield return null;
        }

        ownerMovableObj.m_agent.enabled = true;
        fallCoroutine = null;
    }
}

public class FloatingObjDragEndBehaviour : AbstractInputHandlerBehaviour
{
    public FloatingObjDragEndBehaviour(HoloObject owner)
        : base(owner)
    {

    }
    public override void OnDragStart() { }
    public override AbstractHoloObjInputHandler.MakerType OnDragUpdate(AbstractHoloObjInputHandler.HitObjType hitObjType)
    {
        switch (hitObjType)
        {
            case AbstractHoloObjInputHandler.HitObjType.None:
                return AbstractHoloObjInputHandler.MakerType.None;
            case AbstractHoloObjInputHandler.HitObjType.Book:
            case AbstractHoloObjInputHandler.HitObjType.OtherObj:
            case AbstractHoloObjInputHandler.HitObjType.Character:
                return AbstractHoloObjInputHandler.MakerType.None;
        }

        return AbstractHoloObjInputHandler.MakerType.None;
    }
    public override void OnDragEnd(AbstractHoloObjInputHandler.HitObjType hitObjType) { }
}


