using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractHoloObjInputHandler
{
    public abstract bool OnDragStart();

    public abstract void OnDragEnd();
}

public class MovableObjInputHandler : AbstractHoloObjInputHandler
{
    HoloObject owner;
    AbstractDragEndBehaviour dragEndBehaviour;

    public MovableObjInputHandler(HoloObject owner)
    {
        this.owner = owner;
        if (owner.isFloating) dragEndBehaviour = new FloatingObjDragEndBehaviour(owner);
        else dragEndBehaviour = new GroundingObjDragEndBehaviour(owner);
    }

    public override bool OnDragStart()
    {
        MyObjControllerByBoundingBox.I.SetTargetObject(owner.gameObject);
        return true;
    }

    public override void OnDragEnd()
    {
        dragEndBehaviour.OnDragEnd();
    }
}

public class StaticsObjInputHandler : AbstractHoloObjInputHandler
{
    public override bool OnDragStart() { return false; }
    public override void OnDragEnd() { }
}

public abstract class AbstractDragEndBehaviour
{
    HoloObject owner;
    public AbstractDragEndBehaviour(HoloObject owner)
    {
        this.owner = owner;
    }
    public abstract void OnDragEnd();
}

public class GroundingObjDragEndBehaviour : AbstractDragEndBehaviour
{
    public GroundingObjDragEndBehaviour(HoloObject owner)
        : base(owner)
    {

    }
    public override void OnDragEnd()
    {

    }
}

public class FloatingObjDragEndBehaviour : AbstractDragEndBehaviour
{
    public FloatingObjDragEndBehaviour(HoloObject owner)
        : base(owner)
    {

    }
    public override void OnDragEnd()
    {

    }
}


