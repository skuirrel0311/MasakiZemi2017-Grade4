using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : BaseManager<FlagManager>
{
    List<MyFlag> flagList = new List<MyFlag>();

    public bool GetFlag(string name)
    {
        for (int i = 0; i < flagList.Count; i++)
        {
            if (flagList[i].name == name) return flagList[i].isFlagged;
        }

        return false;
    }

    public void SetFlag(string name, bool isFlagged = true)
    {
        MyFlag myFlag;
        myFlag.isFlagged = isFlagged;
        myFlag.name = name;

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
