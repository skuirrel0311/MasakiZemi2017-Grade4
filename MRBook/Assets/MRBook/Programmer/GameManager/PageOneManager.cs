using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageOneManager : BasePage
{
    ActorManager actorManager;

    public override void PageStart()
    {
        base.PageStart();
        actorManager = ActorManager.I;
    }

    public void ChainBoat()
    {
        actorManager.SetEnableObject("Boat", false);
        actorManager.SetEnableObject("Pile", false);
        actorManager.SetEnableObject("Rope", false);

        HoloObject chainBoat = actorManager.GetObject("AllBoat");
        HoloCharacter urashima = actorManager.GetCharacter(ActorName.Urashima);

        chainBoat.gameObject.SetActive(true);
        urashima.SetParent(chainBoat.transform);
        urashima.ChangeAnimationClip(MotionName.Lie, 0.0f);
    }

    public override void ResetPage()
    {
        base.ResetPage();
        HoloCharacter urashima = actorManager.GetCharacter(ActorName.Urashima);
        HoloObject boat = actorManager.GetObject("Boat");
        urashima.SetParent(boat.transform);
    }
}
