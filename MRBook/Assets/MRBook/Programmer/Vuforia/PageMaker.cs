using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageMaker : MyTracableEventHandler
{
    /// <summary>
    /// どのページに繊維するためのマーカーなのか？
    /// </summary>
    [SerializeField]
    int pageIndex;
    MainGameManager gameManager;

    protected override void Start()
    {
        base.Start();
        gameManager = MainGameManager.I;
    }

    protected override void OnTrackingFound()
    {
        //ページに遷移するべきか？
        if (gameManager.CurrentState != MainGameManager.GameState.Next) return;
        //意味のない移動はしない
        if (gameManager.currentPageIndex == pageIndex) return;

        NotificationManager.I.ShowMessage("page" + (pageIndex + 1) + "に移動します");

        KKUtilities.Delay(2.0f, () =>
        {
            gameManager.ChangePage(pageIndex);
        }, this);

    }

    //見失った時にすることはない
    protected override void OnTrackingLost() { }
}
