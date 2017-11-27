using UnityEngine;
using RootMotion.Dynamics;

public class HoloPuppet : HoloCharacter
{
    [SerializeField]
    BehaviourPuppet m_behavirour = null;

    [SerializeField]
    GameObject rootObj = null;

    [SerializeField]
    PuppetMaster m_puppet = null;

    public GameObject RootObject { get { return rootObj; } }
    public Behaviour PuppetBehaviour { get { return m_behavirour; } }
    public PuppetMaster Puppet { get { return m_puppet; } }

    protected override void InitResetter()
    {
        base.InitResetter();
        resetter.AddBehaviour(new PuppetResetBehaviour(this));
    }

    public override void PlayPage()
    {
        base.PlayPage();
        m_puppet.mode = PuppetMaster.Mode.Active;
    }
}
