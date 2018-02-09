using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MainSceneManager
{
    [SerializeField]
    OffsetController offsetController = null;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("show result");
            PageResultManager.I.SetResult(true);
            PageResultManager.I.ShowResult();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            DisableCurrentPage();
            ChangePage(currentPageIndex + 1);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (CurrentState != GameState.Next)
                DisableCurrentPage();
            else
                ChangePage(currentPageIndex + 1);
        }

        if (Input.GetKeyDown(KeyCode.B)) ChangePage(currentPageIndex - 1);

        if (Input.GetKeyDown(KeyCode.P)) Play();

        if (Input.GetKeyDown(KeyCode.R)) ResetPage();

        //offset controller
        if (Input.GetKeyDown(KeyCode.Keypad4)) offsetController.MoveBook((int)OffsetController.Direction.Left);

        if (Input.GetKeyDown(KeyCode.Keypad6)) offsetController.MoveBook((int)OffsetController.Direction.Right);

        if (Input.GetKeyDown(KeyCode.Keypad7)) offsetController.MoveBook((int)OffsetController.Direction.Down);

        if (Input.GetKeyDown(KeyCode.Keypad9)) offsetController.MoveBook((int)OffsetController.Direction.Up);

        if (Input.GetKeyDown(KeyCode.Keypad8)) offsetController.MoveBook((int)OffsetController.Direction.Front);

        if (Input.GetKeyDown(KeyCode.Keypad2)) offsetController.MoveBook((int)OffsetController.Direction.Back);
    }

    public override void GameStart()
    {
        //エディタ上ではアンカーはないので起動時の位置に固定
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].PageLock(pages[i].transform.position, pages[i].transform.rotation);
        }

        SetPage(currentPageIndex);
        IsGameStart = true;
    }


}
