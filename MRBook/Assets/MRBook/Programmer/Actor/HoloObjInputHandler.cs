using System;
using UnityEngine;
using UnityEngine.AI;
using HoloToolkit.Unity.InputModule;

public class BaseObjInputHandler : MonoBehaviour, IInputClickHandler
{
    public enum HitObjType { None, Book, Character, EventArea, OtherObj }
    public enum MakerType { None, Normal, DontPut, Happen }

    protected HoloObject owner;
    //BoundingBoxの形状を決めるために使用される(トリガーも可)
    public BoxCollider m_collider { get; private set; }
    public float SphereCastRadius { get; protected set; }

    protected HandIconController handIconController;

    Action OnStart;
    Func<HitObjType, MakerType> OnUpdate;
    Action<HitObjType> OnEnd;

    public virtual void Init(HoloObject owner)
    {
        this.owner = owner;
        m_collider = GetComponent<BoxCollider>();
        handIconController = HandIconController.I;

        SetSphreCastRadius();
    }

    /// <summary>
    /// 最初にクリックされた時（バウンディングボックス表示される）
    /// </summary>
    public virtual bool OnClick() { return false; }
    /// <summary>
    /// こいつを選択した状態で別のオブジェクトを選択したとき(バウンディングボックスが消えたとき)
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
        if (behaviour.OnStart != null) OnStart += behaviour.OnStart;
        //Funcを複数Addした場合の挙動は意図したものになるとは限らないので注意
        if (behaviour.OnUpdate != null) OnUpdate += behaviour.OnUpdate;
        if (behaviour.OnEnd != null) OnEnd += behaviour.OnEnd;
    }

    /// <summary>
    /// 掴まれている時に下をチェックする範囲(長さはこのオブジェクトから本までの長さ＋α)
    /// </summary>
    protected virtual void SetSphreCastRadius()
    {
        float colSize = Mathf.Min(m_collider.size.x, m_collider.size.z);
        float scale = Mathf.Min(transform.lossyScale.x, transform.lossyScale.z);
        SphereCastRadius = colSize * scale * 0.5f;
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

    public override void OnDragStart()
    {
        base.OnDragStart();
        handIconController.Hide();
    }

    public override void OnDisabled()
    {
        handIconController.Hide();
    }
}
