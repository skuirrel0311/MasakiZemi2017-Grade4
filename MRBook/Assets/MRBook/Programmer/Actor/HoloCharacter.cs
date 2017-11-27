using UnityEngine;
using UnityEngine.AI;

//目的地を与えると移動してくれる
[RequireComponent(typeof(Animator))]
public class HoloCharacter : HoloMovableObject
{
    public NavMeshAgent m_agent { get; private set; }
    public Animator m_animator { get; private set; }
    [SerializeField]
    protected MotionName defaultMotionName = MotionName.Wait;

    public override Type GetActorType { get { return Type.Character; } }

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
        resetter.AddBehaviour(new LocationResetBehaviour(this));
        resetter.AddBehaviour(new CharacterResetBehaviour(this, defaultMotionName));
    }
    
    public virtual void ChangeAnimationClip(MotionName name, float transitionDuration)
    {
        if (m_animator == null) return;
        
        m_animator.CrossFade(name.ToString(), transitionDuration);
    }
}
