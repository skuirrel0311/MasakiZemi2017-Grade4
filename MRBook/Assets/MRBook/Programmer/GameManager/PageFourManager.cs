﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageFourManager : BasePage
{
    [SerializeField]
    HoloCharacter turtle = null;

    public override void PageStart()
    {
        base.PageStart();

        MainSceneManager.I.OnPageLoaded += AddPageCharacter;

    }

    void AddPageCharacter(BasePage page)
    {
        ActorManager.I.AddCharacter(ActorName.Turtle, turtle);
        MainSceneManager.I.OnPageLoaded -= AddPageCharacter;
    }

}
