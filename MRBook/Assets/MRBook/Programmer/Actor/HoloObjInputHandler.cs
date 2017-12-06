using System;
using UnityEngine;
using UnityEngine.AI;
using HoloToolkit.Unity.InputModule;

public class BaseObjInputHandler : MonoBehaviour, IInputClickHandler
{
    public enum HitObjType { None, Book, Character, OtherObj }
    public enum MakerType { None, Normal, DontPut, PresentItem, DontPresentItem }

    protected HoloObject owner;
    //BoundingBoxの形状を決めるために使用される(トリガーも可)
    public BoxCollider m_collider { get; private set; }
    public float SphereCastRadius { get; protected set; }

    Action OnStart;
    Func<HitObjType, MakerType> OnUpdate;
    Action<HitObjType> OnEnd;

    public virtual void Init(HoloObject owner)
    {
        this.owner = owner;
        m_collider = GetComponent<BoxCollider>();

        SetSphreCastRadius();
    }
    
    /// <summary>
    /// 最初にクリックされた時（バウンディングボックス表示される）
    /// </summary>
    public virtual bool OnClick() { return false; }
    /// <summary>
    /// バウンディングボックスが消えたとき
    /// </summary>
    public virtual void OnDisabled() { }

    public virtual void OnDragStart()
    {
        if (OnStart != null) OnStart.Invoke();
    }
    public virtual MakerType OnDragUpdate(HitObjType hitObjType, HoloObject hitObj)
    {
        if (OnUpdate == null) return MakerType.None;

        return OnUpdate.Invoke(hitObjType);
    }
    public virtual void OnDragEnd(HitObjType hitObjType, HoloObject hitObj)
    {
        if (OnEnd != null) OnEnd.Invoke(hitObjType);
    }

    public void AddBehaviour(AbstractInputHandlerBehaviour behaviour)
    {
        OnStart += behaviour.OnDragStart;
        OnUpdate += behaviour.OnDragUpdate;
        OnEnd += behaviour.OnDragEnd;
    }

    protected virtual void SetSphreCastRadius()
    {
        float colSize = Mathf.Max(m_collider.size.x, m_collider.size.z);
        float scale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        SphereCastRadius = colSize * scale;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        OnClick();
    }
}

public class HoloObjInputHandler : BaseObjInputHandler
{
    public override bool OnClick()
    {
        HandIconController handIconController = HandIconController.I;

        if (owner.ItemSaucer == null) return false;
        if (owner.GetActorType != HoloObject.Type.Character) return false;

        if (!handIconController.IsVisuable)
        {
            handIconController.Init((CharacterItemSaucer)owner.ItemSaucer);
            handIconController.Show();
        }
        else
        {
            handIconController.Hide();
        }
        return false;
    }
}


