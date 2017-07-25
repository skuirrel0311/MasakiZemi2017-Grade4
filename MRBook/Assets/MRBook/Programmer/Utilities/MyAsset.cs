using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAsset<T> where T : Object
{
    public string name;
    public string path;
    public T data { get; private set; }

    public MyAsset(string name, string path)
    {
        this.name = name;
        this.path = path;

        data = Resources.Load<T>(path + name);
    }

    public virtual void Unload()
    {
        data = null;
    }

}
