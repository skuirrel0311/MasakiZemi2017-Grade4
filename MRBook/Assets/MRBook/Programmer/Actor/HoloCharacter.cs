﻿using UnityEngine;
using UnityEngine.AI;

//目的地を与えると移動してくれる
[RequireComponent(typeof(Animator))]
public class HoloCharacter : HoloObject
{
    public NavMeshAgent m_agent { get; private set; }
    public Animator m_animator { get; private set; }
    [SerializeField]
    protected MotionName defaultMotionName = MotionName.Wait;

    public override Type GetActorType { get { return Type.Character; } }

    protected override HoloObjResetter GetResetterInstance() { return new HoloMovableObjResetter(this); }

    protected void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
    }

    protected override void Init()
    {
        ChangeAnimationClip(defaultMotionName, 0.0f);
        base.Init();
    }

    protected override void InitResetter()
    {
        base.InitResetter();
        Resetter.AddBehaviour(new LocationResetBehaviour(this));
        Resetter.AddBehaviour(new CharacterResetBehaviour(this, defaultMotionName));
    }

    public virtual void ChangeAnimationClip(MotionName name, float transitionDuration)
    {
        if (m_animator == null)
        {
            Debug.LogWarning("don't change animation because animator is null");
            return;
        }

        string motionName;

        if(ItemSaucer == null)
            motionName = name.ToString();
        else
            motionName = MotionNameManager.GetMotionName(name, (CharacterItemSaucer)ItemSaucer);

        Debug.Log("call change animation " + motionName);
        m_animator.CrossFade(motionName, transitionDuration);
    }
}
