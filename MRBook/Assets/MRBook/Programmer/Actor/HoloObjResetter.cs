using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractHoloObjResetter
{
    protected HoloObject owner;
    public AbstractHoloObjResetter(HoloObject owner)
    {
        this.owner = owner;
    }
    public abstract void Reset();
}

public class HoloObjResetter : AbstractHoloObjResetter
{
    int defaultLayer;
    bool defaultActive;

    public HoloObjResetter(HoloObject owner)
        :base(owner)
    {
        defaultLayer = owner.gameObject.layer;
        defaultActive = owner.gameObject.activeSelf;
    }
    public override void Reset()
    {
        ObjReset();
    }

    protected void ObjReset()
    {
        owner.gameObject.layer = defaultLayer;
        if (owner.gameObject.activeSelf != defaultActive) owner.gameObject.SetActive(defaultActive);
    }
}

public class MovableObjResetter : HoloObjResetter
{
    Vector3 defaultPosition;
    Quaternion defaultRotation;

    public MovableObjResetter(HoloObject owner)
        :base(owner)
    {
        defaultPosition = owner.transform.position;
        defaultRotation = owner.transform.rotation;
    }

    public void ApplyDefaultTransform()
    {
        defaultPosition = owner.transform.position;
        defaultRotation = owner.transform.rotation;
    }

    public void ApplyDefaultTransform(Vector3 movement)
    {
        defaultPosition += movement;
    }

    public override void Reset()
    {
        LocationReset();
        base.Reset();
    }

    protected void LocationReset()
    {
        owner.transform.position = defaultPosition;
        owner.transform.rotation = defaultRotation;
    }
}

public class ItemResetter : MovableObjResetter
{
    public ItemResetter(HoloObject owner)
        :base(owner)
    {

    }
    public override void Reset()
    {
        base.Reset();
    }
}

public class CharacterResetter : MovableObjResetter
{
    public CharacterResetter(HoloObject owner)
        :base(owner)
    {

    }
    public override void Reset()
    {
        base.Reset();
    }
}

public class PuppetResetter : CharacterResetter
{
    public PuppetResetter(HoloObject owner)
        :base(owner)
    {

    }
    public override void Reset()
    {
        base.Reset();
    }
}

public class AgentResetter : AbstractHoloObjResetter
{
    public AgentResetter(HoloObject owner)
        :base(owner)
    {

    }
    public override void Reset()
    {

    }
}

public class TriangleResetter : AbstractHoloObjResetter
{
    public TriangleResetter(HoloObject owner)
        :base(owner)
    {

    }
    public override void Reset()
    {

    }
}


