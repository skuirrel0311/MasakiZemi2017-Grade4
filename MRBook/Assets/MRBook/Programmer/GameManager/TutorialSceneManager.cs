using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class TutorialSceneManager : BaseManager<TutorialSceneManager>
{
    [SerializeField]
    HoloPuppet urashima = null;

    [SerializeField]
    HoloObject[] objs = null;

    [SerializeField]
    LayerMask layerMask;

    HoloObjResetManager resetManager;

    public Action onReset;

    [SerializeField]
    Rigidbody urashimaFoot = null;

    Vector3 hitPoint;

    protected override void Start()
    {
        base.Start();
        resetManager = new HoloObjResetManager(this);

        urashima.Init();
        resetManager.AddResetter(urashima.Resetter);
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].Init();
            resetManager.AddResetter(objs[i].Resetter);
        }
    }

    public void DeadUrashima()
    {
        HoloObjectController.I.Disable();

        urashima.Puppet.mode = RootMotion.Dynamics.PuppetMaster.Mode.Active;
        urashima.Puppet.state = RootMotion.Dynamics.PuppetMaster.State.Dead;
        Ray ray = new Ray(urashima.transform.position, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 3, layerMask, QueryTriggerInteraction.Collide))
        {
            hitPoint = hit.point;
        }

        //StartCoroutine(MonitorUrashima());
    }

    IEnumerator MonitorUrashima()
    {
        Ray ray = new Ray();
        ray.direction = Vector3.down;
        RaycastHit hit;
        float hitDistance = 1f;

        float timer = 0.0f;

        while (true)
        {
            timer += Time.deltaTime;

            if(timer > 0.2f)
            {
                urashimaFoot.AddForce(urashimaFoot.transform.right * 20.0f, ForceMode.Acceleration);
            }
            ray.origin = urashima.transform.position;

            if (Physics.Raycast(ray, out hit, 3, layerMask))
            {
                if (hit.distance < hitDistance) break;
            }

            yield return null;
        }

        ParticleManager.I.Play("Splash", hit.point);

        yield return new WaitForSeconds(1.0f);

        OnReset();
    }

    public void HitWater()
    {
        ParticleManager.I.Play("Splash", hitPoint);
        Utilities.Delay(1.0f, () => OnReset(), this);
    }

    public void OnReset()
    {
        if (onReset != null) onReset.Invoke();
        resetManager.Reset();
    }
}
