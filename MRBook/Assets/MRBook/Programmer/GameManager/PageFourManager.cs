using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageFourManager : BasePage
{
    [SerializeField]
    HoloCharacter turtle = null;

    [SerializeField]
    HoloItem goldenBall = null;

    public override void PageStart()
    {
        base.PageStart();

        MainSceneManager.I.OnPageInitialized += AddPageCharacter;
    }

    void AddPageCharacter(BasePage page)
    {
        ActorManager.I.AddCharacter(ActorName.Turtle, turtle);
        ActorManager.I.AddObject(goldenBall, false);
        HoloObjResetManager.I.AddResetter(new HoloMovableObjResetter(goldenBall));
        MainSceneManager.I.OnPageInitialized -= AddPageCharacter;
    }

}
