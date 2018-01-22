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

public class BaseImputHandlerBehaviour : AbstractInputHandlerBehaviour
{
    public override Action OnStart { get { return null; } }
    public override Func<BaseObjInputHandler.HitObjType, BaseObjInputHandler.MakerType> OnUpdate { get { return OnDragUpdate; } }
    public override Action<BaseObjInputHandler.HitObjType> OnEnd { get { return OnDragEnd; } }

    public BaseImputHandlerBehaviour(HoloObject owner) : base(owner) { }
    BaseObjInputHandler.MakerType OnDragUpdate(BaseObjInputHandler.HitObjType hitObjType)
    {
        return BaseObjInputHandler.MakerType.None;
    }

    protected virtual void OnDragEnd(BaseObjInputHandler.HitObjType hitObjType)
    {
        if (hitObjType == BaseObjInputHandler.HitObjType.None)
        {
            float bookHeight = OffsetController.I.bookTransform.position.y;
            float airHeight = bookHeight + 0.5f;
            if (owner.transform.position.y < bookHeight)
            {
                Vector3 airPosition = owner.transform.position;
                airPosition.y = airHeight;
                owner.transform.position = airPosition;
            }
        }
    }

}

public class GroundingObjDragEndBehaviour : BaseImputHandlerBehaviour
{
    Coroutine fallCoroutine;
    NavMeshAgent m_agent;

    public override Action OnStart { get { return OnDragStart; } }
    public override Func<BaseObjInputHandler.HitObjType, BaseObjInputHandler.MakerType> OnUpdate { get { return OnDragUpdate; } }

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

    protected override void OnDragEnd(BaseObjInputHandler.HitObjType hitObjType)
    {
        if (hitObjType == BaseObjInputHandler.HitObjType.Book)
        {

            if (fallCoroutine != null)
            {
                owner.StopCoroutine(fallCoroutine);
            }
            fallCoroutine = owner.StartCoroutine(Falling());
        }

        base.OnDragEnd(hitObjType);
    }

    //0.1ずつ落ちて地面が近かったらAgentを有効化する
    IEnumerator Falling()
    {
        NavMeshHit hit;

        float bookHeight = OffsetController.I.bookTransform.position.y;
        float airHeight = bookHeight + 0.5f;
        //Debug.Log("book height = " + bookHeight);

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

            if (owner.transform.position.y < bookHeight)
            {
                Vector3 airPosition = owner.transform.position;
                airPosition.y = airHeight;
                owner.transform.position = airPosition;

                fallCoroutine = null;

                yield break;
            }

            yield return null;
        }

        m_agent.enabled = true;

        yield return null;

        m_agent.enabled = false;
        fallCoroutine = null;
    }
}

public class FloatingObjDragEndBehaviour : BaseImputHandlerBehaviour
{
    public override Action OnStart { get { return null; } }
    public override Func<BaseObjInputHandler.HitObjType, BaseObjInputHandler.MakerType> OnUpdate { get { return OnDragUpdate; } }

    public FloatingObjDragEndBehaviour(HoloObject owner) : base(owner) { }
    BaseObjInputHandler.MakerType OnDragUpdate(BaseObjInputHandler.HitObjType hitObjType)
    {
        //switch (hitObjType)
        //{
        //    case BaseObjInputHandler.HitObjType.None:
        //    case BaseObjInputHandler.HitObjType.Book:
        //    case BaseObjInputHandler.HitObjType.OtherObj:
        //        return BaseObjInputHandler.MakerType.None;
        //    case BaseObjInputHandler.HitObjType.Character:
        //        return BaseObjInputHandler.MakerType.DontPut;
        //}
        return BaseObjInputHandler.MakerType.None;
    }
}
