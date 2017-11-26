using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//すべてのオブジェクトをリセットする人
public class HoloObjResetManager
{
    MonoBehaviour mono;
    List<BaseHoloObjResetter> resetterList = new List<BaseHoloObjResetter>();
    Coroutine resetCoroutine;

    public HoloObjResetManager(MonoBehaviour mono)
    {
        this.mono = mono;
    }

    public void AddResetter(BaseHoloObjResetter resetter)
    {
        resetterList.Add(resetter);
    }

    public void Reset()
    {
        if (resetCoroutine != null)
        {
            mono.StopCoroutine(resetCoroutine);
        }
        resetCoroutine = mono.StartCoroutine(ResetLogic());
    }

    IEnumerator ResetLogic()
    {
        for (int i = 0; i < resetterList.Count; i++)
        {
            resetterList[i].OnDisable();
        }

        yield return null;

        for (int i = 0; i < resetterList.Count; i++)
        {
            resetterList[i].OnLocationReset();
        }

        yield return null;

        for (int i = 0; i < resetterList.Count; i++)
        {
            resetterList[i].OnEnable();
        }

        resetCoroutine = null;
    }
}
//オブジェクトのリセットの手順が書いてあるやつ
public abstract class BaseHoloObjResetter
{
    protected HoloObject owner;
    public BaseHoloObjResetter(HoloObject owner)
    {
        this.owner = owner;
    }

    public abstract void OnDisable();
    public abstract void OnLocationReset();
    public abstract void OnEnable();
}

public class HoloObjResetter : BaseHoloObjResetter
{
    int defaultLayer;
    bool defaultActive;

    public HoloObjResetter(HoloObject owner)
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

public class MovableObjResetter : HoloObjResetter
{
    Vector3 defaultPosition;
    Quaternion defaultRotation;

    public MovableObjResetter(HoloObject owner)
        : base(owner)
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

    public override void OnLocationReset()
    {
        owner.transform.position = defaultPosition;
        owner.transform.rotation = defaultRotation;
    }
}

public class ItemResetter : MovableObjResetter
{
    HoloItem ownerItem;

    public ItemResetter(HoloObject owner)
        : base(owner)
    {
        ownerItem = (HoloItem)owner;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        //アイテムを捨てさせる
    }
}

public class CharacterResetter : MovableObjResetter
{
    HoloCharacter ownerCharacter;
    MotionName defaultMotionName;

    public CharacterResetter(HoloObject owner, MotionName defaultMotionName)
        : base(owner)
    {
        ownerCharacter = (HoloCharacter)owner;
        this.defaultMotionName = defaultMotionName;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        ownerCharacter.ChangeAnimationClip(defaultMotionName, 0.0f);
    }
}

public class PuppetResetter : CharacterResetter
{
    HoloPuppet ownerPuppet;
    public PuppetResetter(HoloObject owner, MotionName defaultMotionName)
        : base(owner, defaultMotionName)
    {
        ownerPuppet = (HoloPuppet)owner;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        ownerPuppet.RootObject.SetActive(false);
    }

    public override void OnLocationReset()
    {
        base.OnLocationReset();
        ownerPuppet.Puppet.state = RootMotion.Dynamics.PuppetMaster.State.Alive;
        ownerPuppet.Puppet.pinWeight = 1.0f;
    }

    public override void OnEnable()
    {
        ownerPuppet.RootObject.SetActive(true);
        base.OnEnable();
    }
}

