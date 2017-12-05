using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HoloMovableObjInputHander  : HoloObjInputHandler
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
        if (!isFloating)
        {
            //NavMeshAgentでRadiusを決める
            NavMeshAgent agent = owner.GetComponent<NavMeshAgent>();
            SphereCastRadius = agent.radius * owner.transform.lossyScale.x;
            return;
        }

        base.SetSphreCastRadius();
    }

    public override bool OnClick()
    {
        MyObjControllerByBoundingBox.I.SetTargetObject(owner);
        base.OnClick();
        return true;
    }
}
