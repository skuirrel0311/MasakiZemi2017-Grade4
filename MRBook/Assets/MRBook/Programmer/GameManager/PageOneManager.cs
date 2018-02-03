using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageOneManager : BasePage
{
    ActorManager actorManager;

    [SerializeField]
    Transform ActorContainer = null;

    public override void PageStart()
    {
        base.PageStart();
        actorManager = ActorManager.I;
    }

    public void ChainBoat()
    {
        //ボートのモデルを入れ替える
        actorManager.SetEnableObject("Boat", false);
        actorManager.SetEnableObject("Pile", false);
        actorManager.SetEnableObject("Rope", false);

        actorManager.SetEnableObject("AllBoat", true);
    }

    public void GameStart()
    {
        bool chainBoat = FlagManager.I.GetFlag("IsChainRope", 0, false);

        Transform parent;

        if (chainBoat)
        {
            parent = actorManager.GetObject("AllBoat").transform;
        }
        else
        {
            parent = actorManager.GetObject("Boat").transform;
        }

        HoloCharacter urashima = actorManager.GetCharacter(ActorName.Urashima);

        Debug.Log("set parent");
        urashima.SetParent(parent);
    }

    public override void ResetPage()
    {
        HoloCharacter urashima = actorManager.GetCharacter(ActorName.Urashima);
        urashima.SetParent(ActorContainer);
        base.ResetPage();
    }
}
