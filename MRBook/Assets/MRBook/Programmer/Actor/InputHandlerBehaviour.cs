using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractInputHandlerBehaviour
{
    protected HoloObject owner;
    public abstract Action OnStart { get; }
    public abstract Func<BaseObjInputHandler.HitObjType, BaseObjInputHandler.MakerType> OnUpdate { get; }
    public abstract Action<BaseObjInputHandler.HitObjType> OnEnd { get; }

    public AbstractInputHandlerBehaviour(HoloObject owner)
    {
        this.owner = owner;
    }
}

public class GroundingObjDragEndBehaviour : AbstractInputHandlerBehaviour
{
    Coroutine fallCoroutine;
    NavMeshAgent m_agent;

    public override Action OnStart { get { return OnDragStart; } }
    public override Func<BaseObjInputHandler.HitObjType, BaseObjInputHandler.MakerType> OnUpdate { get { return OnDragUpdate; } }
    public override Action<BaseObjInputHandler.HitObjType> OnEnd { get { return OnDragEnd; } }

    public GroundingObjDragEndBehaviour(HoloObject owner)
        : base(owner)
    {
        m_agent = owner.GetComponent<NavMeshAgent>();
    }

    void OnDragStart()
    {
        m_agent.enabled = false;
    }

    BaseObjInputHandler.MakerType OnDragUpdate(BaseObjInputHandler.HitObjType hitObjType)
    {
        switch (hitObjType)
        {
            case BaseObjInputHandler.HitObjType.None:
                return BaseObjInputHandler.MakerType.None;
            case BaseObjInputHandler.HitObjType.Book:
                return BaseObjInputHandler.MakerType.Normal;
            case BaseObjInputHandler.HitObjType.OtherObj:
            case BaseObjInputHandler.HitObjType.Character:
                return BaseObjInputHandler.MakerType.DontPut;
        }

        return BaseObjInputHandler.MakerType.None;
    }

    void OnDragEnd(BaseObjInputHandler.HitObjType hitObjType)
    {
        if (hitObjType != BaseObjInputHandler.HitObjType.Book) return;

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
            MyNavMeshBuilder.CreateNavMesh();
            owner.transform.position += Vector3.down * 0.03f;

            //todo : 絵本よりも下に行った場合はやばいのでなにか対応が必要

            if (NavMesh.SamplePosition(owner.transform.position, out hit, m_agent.height, NavMesh.AllAreas))
            {
                Debug.Log("found sample position");
                break;
            }

            yield return null;
        }

        m_agent.enabled = true;

        yield return null;

        m_agent.enabled = false;
        fallCoroutine = null;
    }
}

public class FloatingObjDragEndBehaviour : AbstractInputHandlerBehaviour
{
    public override Action OnStart { get { return null; } }
    public override Func<BaseObjInputHandler.HitObjType, BaseObjInputHandler.MakerType> OnUpdate { get { return OnDragUpdate; } }
    public override Action<BaseObjInputHandler.HitObjType> OnEnd { get { return null; } }

    public FloatingObjDragEndBehaviour(HoloObject owner) : base(owner) { }
    BaseObjInputHandler.MakerType OnDragUpdate(BaseObjInputHandler.HitObjType hitObjType)
    {
        switch (hitObjType)
        {
            case BaseObjInputHandler.HitObjType.None:
            case BaseObjInputHandler.HitObjType.Book:
                return BaseObjInputHandler.MakerType.None;
            case BaseObjInputHandler.HitObjType.OtherObj:
            case BaseObjInputHandler.HitObjType.Character:
                return BaseObjInputHandler.MakerType.DontPut;
        }
        return BaseObjInputHandler.MakerType.None;
    }
}
