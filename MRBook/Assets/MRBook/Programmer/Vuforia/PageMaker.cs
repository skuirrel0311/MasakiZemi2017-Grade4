using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class PageMaker : MyTracableEventHandler
{
    /// <summary>
    /// どのページに繊維するためのマーカーなのか？
    /// </summary>
    [SerializeField]
    int pageIndex = 0;
    MainSceneManager gameManager;

    protected override void Start()
    {
        base.Start();
        gameManager = MainSceneManager.I;
    }

    protected override void OnTrackingFound()
    {
        //ページに遷移するべきか？
        if (gameManager.CurrentState != MainSceneManager.GameState.Next) return;
        //意味のない移動はしない
        if (gameManager.currentPageIndex == pageIndex) return;

        NotificationManager.I.ShowMessage("page" + (pageIndex + 1) + "に移動します");

        Utilities.Delay(2.0f, () =>
        {
            gameManager.ChangePage(pageIndex);
        }, this);

    }

    //見失った時にすることはない
    protected override void OnTrackingLost() { }
}
