using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageOneManager : BasePage
{
    Action onReset;

    [SerializeField]
    GameObject chainBoat = null;

    [SerializeField]
    GameObject boat = null;

    [SerializeField]
    HoloPuppet urashima = null;

    const string boatName = "Boat";
    const string pileName = "Pile";
    const string ropeName = "Rope";

    ActorManager actorManager;

    public override void PageStart()
    {
        base.PageStart();
        actorManager = ActorManager.I;
    }

    public void ChainBoat()
    {
        actorManager.SetEnableObject(boatName, false);
        actorManager.SetEnableObject(pileName, false);
        actorManager.SetEnableObject(ropeName, false);

        chainBoat.SetActive(true);
        urashima.SetParent(chainBoat.transform);
        urashima.ChangeAnimationClip(MotionName.Lie, 0.1f);

        FlagManager.I.SetFlag("IsChainRope", null, true);
        
        onReset += () =>
        {
            actorManager.SetEnableObject(boatName, true);
            actorManager.SetEnableObject(pileName, true);
            actorManager.SetEnableObject(ropeName, true);
            urashima.SetParent(boat.transform);
            chainBoat.SetActive(false);
            FlagManager.I.SetFlag("IsChainRope", null, false);
        };
    }


    public override void ResetPage()
    {
        if(onReset != null) onReset.Invoke();
        onReset = null;
        base.ResetPage();
    }

}
