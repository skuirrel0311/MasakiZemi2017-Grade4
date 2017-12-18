using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : BaseManager<ResultManager>
{
    //玉手箱の中身
    List<HoloItem> secretBoxContentsList = new List<HoloItem>();

    bool isOpenSecretBox;

    //玉手箱の重さ（重すぎると持ち上げられない?）

    int deathCount = 0;

    public void AddSecretBoxContent(HoloItem item)
    {
        secretBoxContentsList.Add(item);
    }

    public void RemoveSecretBoxContent(HoloItem item)
    {
        secretBoxContentsList.Remove(item);
    }

    public void RemoveAllSecretBoxContents()
    {
        secretBoxContentsList.Clear();
    }
    
    public int GetContentKind()
    {
        if (secretBoxContentsList.Count == 1) return 1;
        return 0;
    }

    public void AddDeathCount()
    {
        deathCount++;

        //todo:上限に達したらゲームオーバー
        const int limit = 99;
        if(deathCount > limit)
        {

        }
    }
}
