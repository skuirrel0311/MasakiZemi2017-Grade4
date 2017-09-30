using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineManager : BaseManager<StateMachineManager>
{
    Dictionary<string, IMyTask> taskDictionary = new Dictionary<string, IMyTask>();
    List<string> stopList = new List<string>();

    /// <summary>
    /// キャラの名前とかをキーにするといいんじゃないかな？
    /// </summary>
    /// <param name="name"></param>
    public void Add(string name, IMyTask task)
    {
        Debug.Log("Add Task " + name);
        taskDictionary.Add(name, task);
    }

    void Update()
    {
        foreach (string key in taskDictionary.Keys)
        {
            taskDictionary[key].Update();
        }

        if(stopList.Count != 0) OnStopTask();
    }

    void OnStopTask()
    {
        for (int i = stopList.Count - 1; i >= 0; i--)
        {
            taskDictionary[stopList[i]].Stop();
            taskDictionary.Remove(stopList[i]);
            stopList.Remove(stopList[i]);
        }
    }

    /// <summary>
    /// タスクを中断します
    /// </summary>
    public void Stop(string name)
    {
        if (!taskDictionary.ContainsKey(name)) return;

        stopList.Add(name);
    }
}

public interface IMyTask
{
    void Update();
    void Stop();
}


public class MyTask : IMyTask
{
    public delegate void VoidEvent();

    public VoidEvent OnUpdate;
    public VoidEvent OnExit;

    public MyTask(VoidEvent onUpdate, VoidEvent onExit = null)
    {
        OnUpdate = onUpdate;
        OnExit = onExit;
    }

    public void Update()
    {
        //アップデートは必須なのでnullチェックはしない
        OnUpdate.Invoke();
    }

    public void Stop()
    {
        if (OnExit != null) OnExit.Invoke();
    }
}

