using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//すべてのオブジェクトをリセットする人
public class HoloObjResetManager
{
    MonoBehaviour mono;
    List<HoloObjResetter> resetterList = new List<HoloObjResetter>();
    Coroutine resetCoroutine;

    public HoloObjResetManager(MonoBehaviour mono)
    {
        this.mono = mono;
    }

    public void AddResetter(HoloObjResetter resetter)
    {
        resetterList.Add(resetter);
    }

    public void Reset()
    {
        if (resetCoroutine != null)
        {
            mono.StopCoroutine(resetCoroutine);
        }
        resetCoroutine = mono.StartCoroutine(ObjListResetLogic());
    }

    public void ResetObject(HoloObjResetter resetter)
    {
        mono.StartCoroutine(ObjResetLogic(resetter));
    }

    IEnumerator ObjListResetLogic()
    {
        for (int i = 0; i < resetterList.Count; i++)
        {
            resetterList[i].Disable();
        }

        yield return null;

        for (int i = 0; i < resetterList.Count; i++)
        {
            resetterList[i].LocationReset();
        }

        yield return null;

        for (int i = 0; i < resetterList.Count; i++)
        {
            resetterList[i].Enable();
        }

        resetCoroutine = null;
    }

    IEnumerator ObjResetLogic(HoloObjResetter resetter)
    {
        resetter.Disable();
        yield return null;
        resetter.LocationReset();
        yield return null;
        resetter.Enable();
    }
}

//オブジェクトのリセットの手順が書いてあるやつ
public class HoloObjResetter
{
    Action OnDisable;
    Action OnLocationReset;
    Action OnEnable;

    public void Disable()
    {
        if (OnDisable != null) OnDisable.Invoke();
    }

    public void LocationReset()
    {
        if (OnLocationReset != null) OnLocationReset.Invoke();
    }

    public void Enable()
    {
        if (OnEnable != null) OnEnable.Invoke();
    }
    
    public void AddBehaviour(AbstractHoloObjResetBehaviour behaviour)
    {
        OnDisable += behaviour.OnDisable;
        OnLocationReset += behaviour.OnLocationReset;
        OnEnable += behaviour.OnEnable;
    }
}

//オブジェクトのリセットの手段が書いてあるやつ
public abstract class AbstractHoloObjResetBehaviour
{
    protected HoloObject owner;
    public AbstractHoloObjResetBehaviour(HoloObject owner)
    {
        this.owner = owner;
    }

    public abstract void OnDisable();
    public abstract void OnLocationReset();
    public abstract void OnEnable();
}

public class DefaultHoloObjResetBehaviour : AbstractHoloObjResetBehaviour
{
    int defaultLayer;
    bool defaultActive;

    public DefaultHoloObjResetBehaviour(HoloObject owner)
        : base(owner)
    {
        defaultLayer = owner.gameObject.layer;
        defaultActive = owner.gameObject.activeSelf;
    }

    public override void OnDisable()
    {
        if (owner.gameObject.activeSelf) owner.gameObject.SetActive(false);
    }

    public override void OnLocationReset() { }

    public override void OnEnable()
    {
        owner.gameObject.layer = defaultLayer;
        if (owner.gameObject.activeSelf != defaultActive) owner.gameObject.SetActive(defaultActive);
    }
}

public class LocationResetBehaviour : AbstractHoloObjResetBehaviour
{
    Vector3 defaultPosition;
    Quaternion defaultRotation;

    public LocationResetBehaviour(HoloObject owner)
        : base(owner)
    {
        ApplyDefaultTransform();
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

    public override void OnDisable() { }
    public override void OnLocationReset()
    {
        owner.transform.position = defaultPosition;
        owner.transform.rotation = defaultRotation;
    }
    public override void OnEnable() { }
}

public class ItemResetBehaviour : AbstractHoloObjResetBehaviour
{
    HoloItem ownerItem;

    public ItemResetBehaviour(HoloObject owner)
        : base(owner)
    {
        ownerItem = (HoloItem)owner;
    }

    public override void OnDisable()
    {
        //アイテムを捨てさせる

    }
    public override void OnLocationReset() { }
    public override void OnEnable() { }
}

public class CharacterResetBehaviour : AbstractHoloObjResetBehaviour
{
    HoloCharacter ownerCharacter;
    MotionName defaultMotionName;

    public CharacterResetBehaviour(HoloObject owner, MotionName defaultMotionName)
        : base(owner)
    {
        ownerCharacter = (HoloCharacter)owner;
        this.defaultMotionName = defaultMotionName;
    }

    public override void OnDisable() { }
    public override void OnEnable()
    {
        ownerCharacter.ChangeAnimationClip(defaultMotionName, 0.0f);
    }
    public override void OnLocationReset() { }

}

public class PuppetResetBehaviour : AbstractHoloObjResetBehaviour
{
    HoloPuppet ownerPuppet;
    public PuppetResetBehaviour(HoloObject owner)
        : base(owner)
    {
        ownerPuppet = (HoloPuppet)owner;
    }

    public override void OnDisable()
    {
        ownerPuppet.RootObject.SetActive(false);
    }

    public override void OnLocationReset()
    {
        ownerPuppet.Puppet.state = RootMotion.Dynamics.PuppetMaster.State.Alive;
        ownerPuppet.Puppet.pinWeight = 1.0f;
    }

    public override void OnEnable()
    {
        ownerPuppet.RootObject.SetActive(true);
    }
}

