﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MainSceneManager
{
    [SerializeField]
    OffsetController offsetController = null;

    protected override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!IsGameStart) GameStart();
        }

        if (Input.GetKeyDown(KeyCode.N)) ChangePage(currentPageIndex + 1);

        if (Input.GetKeyDown(KeyCode.B)) ChangePage(currentPageIndex - 1);

        if (Input.GetKeyDown(KeyCode.P)) Play();

        if (Input.GetKeyDown(KeyCode.R)) ResetPage();

        if (Input.GetKeyDown(KeyCode.Alpha4)) offsetController.MoveBook((int)OffsetController.Direction.Left);

        if (Input.GetKeyDown(KeyCode.Alpha6)) offsetController.MoveBook((int)OffsetController.Direction.Right);

        if (Input.GetKeyDown(KeyCode.Alpha2)) offsetController.MoveBook((int)OffsetController.Direction.Down);

        if (Input.GetKeyDown(KeyCode.Alpha8)) offsetController.MoveBook((int)OffsetController.Direction.Up);

        if (Input.GetKeyDown(KeyCode.Alpha7)) offsetController.MoveBook((int)OffsetController.Direction.Front);

        if (Input.GetKeyDown(KeyCode.Alpha9)) offsetController.MoveBook((int)OffsetController.Direction.Back);
    }

    public override void GameStart()
    {
        //エディタ上ではアンカーはないので起動時の位置に固定
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].PageLock(pages[i].transform.position, pages[i].transform.rotation, i);
        }

        SetPage(currentPageIndex);

        IsGameStart = true;
        if (OnGameStart != null) OnGameStart.Invoke();
    }


}
