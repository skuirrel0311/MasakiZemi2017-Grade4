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
    MainSceneManager sceneManager;

    bool isBooking = false;

    protected override void Start()
    {
        base.Start();
        sceneManager = MainSceneManager.I;
    }

    protected override void OnTrackingFound()
    {
        if (isBooking) return;

        if (sceneManager.CurrentState == MainSceneManager.GameState.NextWait)
        {
            if (sceneManager.currentPageIndex == pageIndex)
            {
                sceneManager.DisableCurrentPage();
                return;
            }
            else if (sceneManager.currentPageIndex + 1 == pageIndex)
            {
                //次のページを先に見つけてしまった
                sceneManager.DisableCurrentPage(() =>
                {
                    //前のページを消した後に自動的にこのページを表示させる
                    sceneManager.ChangePage(pageIndex);
                });
            }
        }
        else if (sceneManager.CurrentState == MainSceneManager.GameState.Next)
        {
            if (sceneManager.currentPageIndex != pageIndex) return;

            //まだフェードイン中だった
            if (Fader.I.CurrentState == Fader.State.FadeIn)
            {
                isBooking = true;
                Fader.I.AddCallBack(() =>
                {
                    isBooking = false;
                    sceneManager.ChangePage(pageIndex);
                });
            }
            else
            {
                sceneManager.ChangePage(pageIndex);
            }


        }
    }

    //見失った時にすることはない
    protected override void OnTrackingLost() { }
}
