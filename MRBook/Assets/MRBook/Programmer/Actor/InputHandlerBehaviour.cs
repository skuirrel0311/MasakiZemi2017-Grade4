using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractInputHandlerBehaviour
{
    protected HoloObject owner;
    public AbstractInputHandlerBehaviour(HoloObject owner)
    {
        this.owner = owner;
    }
    public abstract void OnDragStart();
    public abstract BaseObjInputHandler.MakerType OnDragUpdate(BaseObjInputHandler.HitObjType hitObjType);
    public abstract void OnDragEnd(BaseObjInputHandler.HitObjType hitObjType);
}

public class GroundingObjDragEndBehaviour : AbstractInputHandlerBehaviour
{
    HoloMovableObject ownerMovableObj;
    Coroutine fallCoroutine;
    NavMeshAgent m_agent;
    public GroundingObjDragEndBehaviour(HoloObject owner)
        : base(owner)
    {
        ownerMovableObj = (HoloMovableObject)owner;
        m_agent = owner.GetComponent<NavMeshAgent>();
    }

    public override void OnDragStart()
    {
        m_agent.enabled = false;
    }
    public override BaseObjInputHandler.MakerType OnDragUpdate(BaseObjInputHandler.HitObjType hitObjType)
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
    public override void OnDragEnd(BaseObjInputHandler.HitObjType hitObjType)
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
            owner.transform.position += Vector3.down * 0.1f;

            //todo : 絵本よりも下に行った場合はやばいのでなにか対応が必要

            if (NavMesh.SamplePosition(owner.transform.position, out hit, m_agent.height, NavMesh.AllAreas))
            {
                break;
            }

            yield return null;
        }

        m_agent.enabled = true;
        fallCoroutine = null;
    }
}

public class FloatingObjDragEndBehaviour : AbstractInputHandlerBehaviour
{
    public FloatingObjDragEndBehaviour(HoloObject owner) : base(owner) { }
    public override void OnDragStart() { }
    public override BaseObjInputHandler.MakerType OnDragUpdate(BaseObjInputHandler.HitObjType hitObjType)
    {
        switch (hitObjType)
        {
            case BaseObjInputHandler.HitObjType.None:
                return BaseObjInputHandler.MakerType.None;
            case BaseObjInputHandler.HitObjType.Book:
            case BaseObjInputHandler.HitObjType.OtherObj:
            case BaseObjInputHandler.HitObjType.Character:
                return BaseObjInputHandler.MakerType.None;
        }

        return BaseObjInputHandler.MakerType.None;
    }
    public override void OnDragEnd(BaseObjInputHandler.HitObjType hitObjType) { }
}
