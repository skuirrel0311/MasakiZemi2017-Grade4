using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

//すべてのオブジェクトをリセットする人
public class HoloObjResetManager
{
    public static HoloObjResetManager I = null;

    MonoBehaviour mono;
    List<HoloObjResetter> resetterList = new List<HoloObjResetter>();
    List<HoloMovableObjResetter> movableResetterList = new List<HoloMovableObjResetter>();
    Coroutine resetCoroutine;

    public HoloObjResetManager(MonoBehaviour mono)
    {
        if (I != null) return;

        I = this;
        this.mono = mono;
    }

    public void AddResetter(HoloObjResetter resetter)
    {
        resetterList.Add(resetter);
    }

    public void AddMovableResetter(HoloMovableObjResetter resetter)
    {
        movableResetterList.Add(resetter);
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

        Debug.Log("on disable");
        for (int i = 0; i < resetterList.Count; i++)
        {
            resetterList[i].Disable();
        }

        yield return mono.StartCoroutine(Utilities.Delay(10, () =>
        {
            Debug.Log("on location reset");
            for (int i = 0; i < resetterList.Count; i++)
            {
                resetterList[i].LocationReset();
            }
        }));
        
        yield return mono.StartCoroutine(Utilities.Delay(10, () =>
        {
            Debug.Log("on enable");
            for (int i = 0; i < resetterList.Count; i++)
            {
                resetterList[i].Enable();
            }
        }));
        
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

public class HoloMovableObjResetter : HoloObjResetter
{
    LocationResetBehaviour locationResetBehaviour;

    //LocationResetBehaviourを重複してAddしてしまうことを防ぐ
    public void AddBehaviour(LocationResetBehaviour locationResetBehaviour)
    {
        Debug.Log("already addition location reset behaviour");
    }

    public HoloMovableObjResetter(HoloObject owner)
    {
        locationResetBehaviour = new LocationResetBehaviour(owner);
        base.AddBehaviour(locationResetBehaviour);
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
    Transform ownerTransform;
    Transform defaultParent;

    public LocationResetBehaviour(HoloObject owner)
        : base(owner)
    {
        ownerTransform = owner.transform;
        defaultParent = ownerTransform.parent;
        defaultPosition = ownerTransform.localPosition;
        defaultRotation = ownerTransform.localRotation;
    }

    public override void OnDisable() { }
    public override void OnLocationReset()
    {
        owner.transform.parent = defaultParent;
        ownerTransform.localPosition = defaultPosition;
        ownerTransform.localRotation = defaultRotation;
    }
    public override void OnEnable() { }
}

public class ItemResetBehaviour : AbstractHoloObjResetBehaviour
{
    HoloItem ownerItem;
    HoloObject defaultItemOwner;
    Collider[] cols;

    public ItemResetBehaviour(HoloObject owner, HoloObject defaultItemOwner)
        : base(owner)
    {
        ownerItem = (HoloItem)owner;
        this.defaultItemOwner = defaultItemOwner;
    }

    public override void OnDisable()
    {
        //アイテムを捨てさせる
        if (ownerItem.owner == null) return;
        ownerItem.owner.ItemSaucer.DumpItem(ownerItem, false);
    }
    public override void OnLocationReset() { }
    public override void OnEnable()
    {
        ownerItem.SetColliderEnable(true);

        if (defaultItemOwner == null) return;

        defaultItemOwner.ItemSaucer.SetItem(ownerItem, false);
    }
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
        ownerPuppet.Puppet.mode = RootMotion.Dynamics.PuppetMaster.Mode.Active;
        ownerPuppet.Puppet.state = RootMotion.Dynamics.PuppetMaster.State.Alive;
        ownerPuppet.Puppet.pinWeight = 1.0f;
    }

    public override void OnLocationReset()
    {
        ownerPuppet.RootObject.SetActive(true);
    }

    public override void OnEnable()
    {
        ownerPuppet.Puppet.mode = RootMotion.Dynamics.PuppetMaster.Mode.Disabled;
    }
}

