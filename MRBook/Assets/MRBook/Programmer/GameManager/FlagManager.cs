using System.Collections.Generic;

using UnityEngine;

public class FlagManager : BaseManager<FlagManager>
{
    MainSceneManager sceneManager;
    Dictionary<string, MyFlag> flagDictionary = new Dictionary<string, MyFlag>();


    protected override void Start()
    {
        base.Start();
        sceneManager = MainSceneManager.I;

        sceneManager.OnPlayPage += OnPlayPage;
    }

    void OnPlayPage(BasePage page)
    {
        SetCurrentPageFlag();
    }

    void SetCurrentPageFlag()
    {
        //イベントのトリガーをチェックしていく
        GameObject[] eventTriggerObjects = GameObject.FindGameObjectsWithTag("Trigger");

        for (int i = 0; i < eventTriggerObjects.Length; i++)
        {
            MyEventTrigger[] eventTriggers = eventTriggerObjects[i].GetComponents<MyEventTrigger>();

            for (int j = 0; j < eventTriggers.Length; j++)
            {
                eventTriggers[j].SetFlag();
            }
        }
    }

    public bool GetFlag(string name, bool isCheckNow)
    {
        MyFlag myFlag;
        if (flagDictionary.TryGetValue(name, out myFlag))
        {
            //いま判定を行う
            if (isCheckNow) myFlag.eventTrigger.SetFlag();

            return myFlag.isFlagged;
        }

        //見つからなかった
        Debug.LogWarning(name + " is not found");

        //強制的にfalse
        return false;
    }

    public bool GetFlag(string name, int pageIndex, bool isCheckNow = false)
    {
        name = name + pageIndex;
        return GetFlag(name, isCheckNow);
    }

    public virtual void SetFlag(string name,MyEventTrigger eventTrigger, bool isFlagged = true)
    {
        name = name + sceneManager.currentPageIndex;

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
