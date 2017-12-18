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

    public void ChainBoat()
    {
        ActorManager.I.SetEnableObject(boatName, false);
        ActorManager.I.SetEnableObject(pileName, false);
        ActorManager.I.SetEnableObject(ropeName, false);

        chainBoat.SetActive(true);

        onReset += () =>
        {
            ActorManager.I.SetEnableObject(boatName, true);
            ActorManager.I.SetEnableObject(pileName, true);
            ActorManager.I.SetEnableObject(ropeName, true);
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
