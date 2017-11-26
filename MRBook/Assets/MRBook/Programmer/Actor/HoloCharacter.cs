using UnityEngine;

//目的地を与えると移動してくれる
[RequireComponent(typeof(Animator))]
public class HoloCharacter : HoloMovableObject
{
    public Animator m_animator { get; private set; }
    [SerializeField]
    protected MotionName defaultMotionName = MotionName.Wait;
    
    public virtual bool CanHaveItem { get { return false; } }

    public override Type GetActorType { get { return Type.Character; } }

    protected override void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    protected override void Init()
    {
        ChangeAnimationClip(defaultMotionName, 0.0f);
        base.Init();
    }

    protected override void InitResetter()
    {
        resetter = new CharacterResetter(this, defaultMotionName);
    }
    
    public virtual void ChangeAnimationClip(MotionName name, float transitionDuration)
    {
        if (m_animator == null) return;
        
        m_animator.CrossFade(name.ToString(), transitionDuration);
    }
}
