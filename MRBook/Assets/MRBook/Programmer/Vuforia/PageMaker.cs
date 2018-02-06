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

    protected override void Start()
    {
        base.Start();
        sceneManager = MainSceneManager.I;
    }

    protected override void OnTrackingFound()
    {
        if (sceneManager.CurrentState == MainSceneManager.GameState.NextWait)
        {

            if (sceneManager.currentPageIndex != pageIndex)
            {
                //見つけたいマーカーじゃなかった（まずいかも）
                //todo:見つけたいページの次のマーカーを見つけた場合もホログラムを消す
                return;
            }

            sceneManager.DisableCurrentPage();
        }
        else if(sceneManager.CurrentState == MainSceneManager.GameState.Next)
        {
            if (sceneManager.currentPageIndex + 1 != pageIndex)
            {
                //見つけたいマーカーじゃなかった（まずいかも）
                return;
            }

            sceneManager.ChangePage(pageIndex);
        }
    }

    //見失った時にすることはない
    protected override void OnTrackingLost() { }
}
