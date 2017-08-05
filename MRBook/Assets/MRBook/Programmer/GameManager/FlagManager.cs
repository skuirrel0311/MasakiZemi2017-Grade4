using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : BaseManager<FlagManager>
{
    List<MyFlag> flagList = new List<MyFlag>();

    public bool GetFlag(string name, bool isCheckNow)
    {
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
        myFlag.name = name + MainGameManager.I.currentPageIndex;
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
