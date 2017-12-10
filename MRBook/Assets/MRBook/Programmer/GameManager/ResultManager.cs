using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : BaseManager<ResultManager>
{
    //玉手箱の中身
    List<HoloItem> secretBoxContentsList = new List<HoloItem>();

    //玉手箱の重さ（重すぎると持ち上げられない?）

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
}
