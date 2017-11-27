using UnityEngine;
using UnityEngine.AI;

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
            SphereCastRadius = agent.radius;
            return;
        }

        base.SetSphreCastRadius();
    }

    public override void OnClick()
    {
        MyObjControllerByBoundingBox.I.SetTargetObject(owner.gameObject);
    }
}


