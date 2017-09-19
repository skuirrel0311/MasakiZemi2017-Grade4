using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;

public class BookPositionModifier : BaseManager<BookPositionModifier>
{
    MyGameManager gameManager;
    MainSceneManager mainSceneManager;

    protected override void Start()
    {
        base.Start();

        gameManager = MyGameManager.I;
        mainSceneManager = MainSceneManager.I;

        WorldManager.OnPositionalLocatorStateChanged += WorldManagerOnStateChanged;
    }

    void WorldManagerOnStateChanged(PositionalLocatorState oldState, PositionalLocatorState newState)
    {
        //アクティブに戻った時になおす
        if (newState == PositionalLocatorState.Active)
        {
            ModifyBookPosition(true);
        }
    }

    public void ModifyBookPosition(bool showDialog)
    {
        if (gameManager.currentSceneState != MyGameManager.SceneState.Main) return;
        //MainSceneManager.I.SetBookPositionByAnchor(bookTransform.position, bookTransform.rotation);
        if (showDialog) NotificationManager.I.ShowDialog("警告", "ホログラムのずれを検知しました。", true, 3.0f);
    }
}
