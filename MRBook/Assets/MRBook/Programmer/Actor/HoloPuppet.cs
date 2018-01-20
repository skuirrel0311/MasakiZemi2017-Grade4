using System.Collections;
using UnityEngine;
using RootMotion.Dynamics;
using KKUtilities;

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

    Coroutine monitorPuppetCoroutine;

    SkinnedMeshRenderer rend;
    Mesh defaultMesh;
    Material defaultMat;

    public override void Init()
    {
        base.Init();

        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        defaultMesh = rend.sharedMesh;
        defaultMat = rend.material;

        MainSceneManager.I.OnReset += () =>
        {
            rend.sharedMesh = defaultMesh;
            rend.material = defaultMat;
        };
    }

    protected override void SetDefaultParent()
    {
        defaultParent = rootObj.transform.parent;
    }

    protected override void InitResetter()
    {
        base.InitResetter();
        Resetter.AddBehaviour(new PuppetResetBehaviour(this));

        MainSceneManager.I.OnPlayEnd += (success) =>
        {
            if(monitorPuppetCoroutine != null)
            {
                StateMachineManager.I.StopCoroutine(monitorPuppetCoroutine);
            }

            monitorPuppetCoroutine = null;
        };
    }

    public override void PlayPage()
    {
        base.PlayPage();
        m_puppet.mode = PuppetMaster.Mode.Active;
        monitorPuppetCoroutine = StateMachineManager.I.StartCoroutine(MonitorPuppet()); 
    }

    IEnumerator MonitorPuppet()
    {
        Vector3 particlePosition;
        while (true)
        {
            yield return null;
            if (Puppet.urashimaState == PuppetMaster.UrashimaState.Dead) break;
        }

        Utilities.Delay(2.0f, () =>
        {
            particlePosition = transform.position;
            ParticleManager.I.Play("UrashimaSoul", particlePosition);
            AkSoundEngine.PostEvent("Die", gameObject);
            ResultManager.I.AddDeathCount();
        }, ParticleManager.I);

        monitorPuppetCoroutine = null;
    }

    public override void ChangeScale(float scaleRate)
    {
        rootObj.transform.position = transform.position;
        transform.localPosition = Vector3.zero;
        rootObj.transform.localScale = rootObj.transform.localScale * scaleRate;
    }

    public override void SetParent(Transform parent)
    {
        rootObj.transform.parent = parent;
    }
}
