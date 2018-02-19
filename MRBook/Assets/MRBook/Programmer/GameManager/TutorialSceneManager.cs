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
    HoloObject book = null;

    [SerializeField]
    HoloObject[] objs = null;

    [SerializeField]
    LayerMask layerMask;

    HoloObjResetManager resetManager;

    public Action onReset;

    Vector3 hitPoint;

    bool hitWater = false;

    [SerializeField]
    GameObject tutorial = null;
    [SerializeField]
    GameObject tutorialPosition = null;

    protected override void Start()
    {
        base.Start();
        resetManager = new HoloObjResetManager(this);

        urashima.Init();
        resetManager.AddResetter(urashima.Resetter);
        book.Init();
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].Init();
            resetManager.AddResetter(objs[i].Resetter);
        }
    }

    public void SetTutorial()
    {
        tutorialPosition.SetActive(false);
        tutorial.transform.SetPositionAndRotation(tutorialPosition.transform.position, tutorialPosition.transform.rotation);
        Utilities.Delay(10, ()=> tutorial.SetActive(true), this);
    }

    public void ActiveDebugMode()
    {
        tutorial.SetActive(false);
        tutorialPosition.SetActive(true);
    }

    public void DeadUrashima()
    {
        HoloObjectController.I.Disable();

        Ray ray = new Ray(urashima.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3, layerMask, QueryTriggerInteraction.Collide))
        {
            hitPoint = hit.point;
            urashima.Puppet.mode = RootMotion.Dynamics.PuppetMaster.Mode.Active;
            urashima.Puppet.state = RootMotion.Dynamics.PuppetMaster.State.Dead;
            Utilities.Delay((hit.distance * 1.0f) + 2.0f, () =>
           {
               if (hitWater)
               {
                   ParticleManager.I.Play("UrahsimaSoul", hit.point);
               }
               else
               {
                   ParticleManager.I.Play("UrahsimaSoul", urashima.transform.position);
               }
               AkSoundEngine.PostEvent("Die", gameObject);
               OnReset();
           }, this);
        }
    }

    public void HitWater()
    {
        ParticleManager.I.Play("Splash", hitPoint);
        AkSoundEngine.PostEvent("Splash", gameObject);
    }

    public void OnReset()
    {
        if (onReset != null) onReset.Invoke();
        resetManager.Reset();
    }
}
