using System;
using System.Collections;
using System.Collections.Generic;

public class FlagManager : BaseManager<FlagManager>
{
    MainGameManager gameManager;
    List<MyFlag> flagList = new List<MyFlag>();

    protected override void Start()
    {
        base.Start();
        gameManager = MainGameManager.I;
    }

    public bool GetFlag(string name, bool isCheckNow)
    {
        name += gameManager.currentPageIndex;
        for (int i = 0; i < flagList.Count; i++)
        {
            if (flagList[i].name == name)
            {
                if (isCheckNow) flagList[i].eventTrigger.SetFlag();
                return flagList[i].isFlagged;
            }
        }

        return false;
    }

    public void SetFlag(string name,MyEventTrigger eventTrigger, bool isFlagged = true)
    {
        MyFlag myFlag;
        myFlag.isFlagged = isFlagged;
        //同名のフラグは同じページには存在しない
        myFlag.name = name + gameManager.currentPageIndex;
        myFlag.eventTrigger = eventTrigger;

        for (int i = 0; i < flagList.Count; i++)
        {
            if (flagList[i].name == name)
            {
                flagList[i] = myFlag;
                return;
            }
        }

        flagList.Add(myFlag);
    }
}
