using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageOneManager : BasePage
{
    Action onReset;

    [SerializeField]
    GameObject chainBoat = null;

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

        onReset += () =>
        {
            actorManager.SetEnableObject(boatName, true);
            actorManager.SetEnableObject(pileName, true);
            actorManager.SetEnableObject(ropeName, true);
            chainBoat.SetActive(false);
        };
    }


    public override void ResetPage()
    {
        if(onReset != null) onReset.Invoke();
        onReset = null;
        base.ResetPage();
    }

}
