using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAssetStore : BaseManager<MyAssetStore>
{
    Dictionary<string, IMyAsset> assetDictionray = new Dictionary<string, IMyAsset>();

    /// <summary>
    /// パスの最後には「/」を付ける
    /// </summary>
    public T GetAsset<T>(string name, string path) where T : Object
    {
        //既にロードされているならそのまま
        MyAsset<T> asset = FindAsset<T>(name);
        if (asset != null) return asset.Data;

        //ロードしてリストに追加する
        T data = Resources.Load<T>(path + name);
        if (data == null)
        {
            Debug.LogError("not found " + name);
            return null;
        }
        asset = new MyAsset<T>(name, path, data);
        assetDictionray.Add(name, asset);

        return asset.Data;
    }

    MyAsset<T> FindAsset<T>(string name) where T : Object
    {
        IMyAsset asset;
        if (assetDictionray.TryGetValue(name, out asset))
        {
            return (MyAsset<T>)asset;
        }
        else
        {
            return null;
        }
    }
}
