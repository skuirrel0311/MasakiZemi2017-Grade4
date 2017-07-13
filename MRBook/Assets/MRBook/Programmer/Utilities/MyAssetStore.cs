using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAssetStore<T> where T : Object
{
    List<MyAsset<T>> assetList = new List<MyAsset<T>>();
    string rootPath;

    public MyAssetStore(string rootPath)
    {
        this.rootPath = rootPath;
    }

    /// <summary>
    /// パスの最後には「/」を付ける
    /// </summary>
    /// <param name="name"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public T GetAsset(string name, string path = "")
    {
        //既にロードされているならそのまま
        MyAsset<T> asset = FindAsset(name);
        if (asset != null) return asset.data;

        //ロードしてリストに追加する
        if (path == "") path = rootPath;
        asset = new MyAsset<T>(name, path);
        assetList.Add(asset);

        return asset.data;
    }

    MyAsset<T> FindAsset(string name)
    {
        for(int i = 0;i < assetList.Count;i++)
        {
            if (assetList[i].name == name) return assetList[i];
        }
        return null;
    }
}
