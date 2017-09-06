using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MainSceneManager
{
    protected override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            if (!IsGameStart) GameStart();
        }

        if (Input.GetKeyDown(KeyCode.N)) ChangePage(currentPageIndex + 1);

        if (Input.GetKeyDown(KeyCode.B)) ChangePage(currentPageIndex - 1);

        if (Input.GetKeyDown(KeyCode.P)) Play();

        if (Input.GetKeyDown(KeyCode.R)) ResetPage();
    }
    
    public override void GameStart(Transform bookTransfomr = null)
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
