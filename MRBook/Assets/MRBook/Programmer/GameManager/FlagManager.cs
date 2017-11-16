using System.Collections.Generic;

using UnityEngine;

public class FlagManager : BaseManager<FlagManager>
{
    MainSceneManager gameManager;
    Dictionary<string, MyFlag> flagDictionary = new Dictionary<string, MyFlag>();

    protected override void Start()
    {
        base.Start();
#if UNITY_EDITOR
        gameManager = TestSceneManager.I;
#else
        gameManager = MainSceneManager.I;
#endif
    }

    public bool GetFlag(string name, bool isCheckNow)
    {
        MyFlag myFlag;
        name = name + gameManager.currentPageIndex;
        if (flagDictionary.TryGetValue(name, out myFlag))
        {
            //いま判定を行う
            if (isCheckNow) myFlag.eventTrigger.SetFlag();

            return myFlag.isFlagged;
        }

        //見つからなかった
        Debug.LogError(name + " is not found");

        //強制的にfalse
        return false;
    }

    public virtual void SetFlag(string name,MyEventTrigger eventTrigger, bool isFlagged = true)
    {
        name = name + gameManager.currentPageIndex;

        MyFlag myFlag;
        
        if(flagDictionary.TryGetValue(name, out myFlag))
        {
            //既にあったらフラグの値だけ更新
            myFlag.isFlagged = isFlagged;
            flagDictionary[name] = myFlag;
        }
        else
        {
            myFlag.isFlagged = isFlagged;
            myFlag.eventTrigger = eventTrigger;
            myFlag.name = name;
            flagDictionary.Add(name, myFlag);
        }
    }
}
