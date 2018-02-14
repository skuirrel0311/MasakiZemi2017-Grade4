using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSceneManager : BaseManager<TutorialSceneManager>
{
    [SerializeField]
    HoloPuppet urashima = null;

    [SerializeField]
    HoloObject[] objs = null;

    [SerializeField]
    LayerMask layerMask;

    protected override void Start()
    {
        base.Start();
        urashima.Init();

        for(int i = 0;i< objs.Length;i++)
        {
            objs[i].Init();
        }
    }

    public void DeadUrashima()
    {
        urashima.Puppet.mode = RootMotion.Dynamics.PuppetMaster.Mode.Active;
        urashima.Puppet.state = RootMotion.Dynamics.PuppetMaster.State.Dead;

        StartCoroutine(MonitorUrashima());
    }

    IEnumerator MonitorUrashima()
    {
        while(true)
        {
            Collider[] cols = Physics.OverlapSphere(urashima.transform.position, 0.1f, layerMask);

            if (cols.Length != 0) break;
            yield return null;
        }

        ParticleManager.I.Play("Splash", urashima.transform.position);
    }
}
