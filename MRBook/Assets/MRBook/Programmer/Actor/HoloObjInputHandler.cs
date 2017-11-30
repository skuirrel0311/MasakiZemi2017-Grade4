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
    
    public virtual void OnClick() { }
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
    [SerializeField]
    bool isFloating = false;

    public override void Init(HoloObject owner)
    {
        if (isFloating) AddBehaviour(new FloatingObjDragEndBehaviour(owner));
        else AddBehaviour(new GroundingObjDragEndBehaviour(owner));
        base.Init(owner);
    }

    protected override void SetSphreCastRadius()
    {
        if(!isFloating)
        {
            //NavMeshAgentでRadiusを決める
            NavMeshAgent agent = owner.GetComponent<NavMeshAgent>();
            SphereCastRadius = agent.radius * owner.transform.lossyScale.x;
            return;
        }

        base.SetSphreCastRadius();
    }

    public override void OnClick()
    {
        MyObjControllerByBoundingBox.I.SetTargetObject(owner.gameObject);
    }
}


