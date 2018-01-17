using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class PageFiveManager : BasePage
{
    [SerializeField]
    HoloCharacter urashima = null;
    [SerializeField]
    HoloCharacter turtle = null;
    [SerializeField]
    HoloItem secretBox_Box = null;

    public override void PageStart()
    {
        base.PageStart();

        MainSceneManager sceneManager = MainSceneManager.I;

        sceneManager.OnPageLoaded += AddPageCharacter;

        sceneManager.OnPlayEnd += (success) =>
        {
            ResultManager.I.ShowTotalResult();

            Utilities.Delay(2.0f, () =>
            {
                ResultManager.I.ShowTitleBack();
            }, this);

        };
    }

    void AddPageCharacter(BasePage page)
    {
        ActorManager.I.AddCharacter(ActorName.Urashima, urashima);
        ActorManager.I.AddCharacter(ActorName.Turtle, turtle);
        ActorManager.I.AddObject(secretBox_Box);
        MainSceneManager.I.OnPageLoaded -= AddPageCharacter;
    }
}
