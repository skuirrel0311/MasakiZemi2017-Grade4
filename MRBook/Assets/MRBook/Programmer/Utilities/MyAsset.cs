using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMyAsset
{
    string Name { get; }
    string Path { get; }
}

public class MyAsset<T> : IMyAsset
    where T : Object
{
    public string Name { get; private set; }
    public string Path { get; private set; }
    public T Data { get; private set; }

    public MyAsset(string name, string path, T data)
    {
        Name = name;
        Path = path;
        Data = data;
    }

    public virtual void Unload()
    {
        Data = null;
    }
}
