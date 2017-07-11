using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MainGameManager
{
    protected override void Start()
    {
        base.Start();
        GameStart();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) ChangePage(currentPageIndex + 1);

        if (Input.GetKeyDown(KeyCode.B)) ChangePage(currentPageIndex - 1);

        if (Input.GetKeyDown(KeyCode.P)) Play();

        if (Input.GetKeyDown(KeyCode.R)) ResetPage();
    }
    
    public override void GameStart()
    {
        //エディタ上ではアンカーはないので起動時の位置に固定
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].PageLock(pages[i].transform.position, pages[i].transform.rotation, i);
        }

        SetPage(currentPageIndex);

        //使うUIだけアクティブにしておく
        uiController.missionText.gameObject.SetActive(true);
        uiController.stateText.gameObject.SetActive(true);
    }
}
