using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineManager : BaseManager<StateMachineManager>
{
    Dictionary<string, IMyTask> taskDictionary = new Dictionary<string, IMyTask>();
    List<string> stopList = new List<string>();

    Dictionary<int, AnimatorBakedData.MyAnimatorState> animatorStateDictionary = new Dictionary<int, AnimatorBakedData.MyAnimatorState>();

    int currentPageIndex = -1;
    public Dictionary<int, AnimatorBakedData.MyAnimatorState> AnimatorStateDictionary
    {
        get
        {
            //if (currentPageIndex == MainSceneManager.I.currentPageIndex) return animatorStateDictionary;
            if (currentPageIndex == 0) return animatorStateDictionary;
            animatorStateDictionary.Clear();
            
            AnimatorBakedData bakeData = Resources.Load<AnimatorBakedData>("Data/" + MainSceneManager.I.m_Animator.runtimeAnimatorController.name);

            for (int i = 0; i < bakeData.hashList.Count; i++)
            {
                animatorStateDictionary.Add(bakeData.hashList[i], bakeData.stateList[i]);
            }

            return animatorStateDictionary;
        }
    }


    protected override void Start()
    {
        base.Start();
        MainSceneManager.I.OnPlayEnd += (success) => StopAll();
    }

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
            if (taskDictionary[key].Update() != BehaviourStatus.Running)
            {
                Debug.Log(key + "is stop");
                Stop(key);
            }
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
        if (!taskDictionary.ContainsKey(name))
        {
            Debug.LogWarning(name + "is already stop or didn't addition");

            if(taskDictionary.Count == 0)
            {
                Debug.Log("task count is 0");
                return;
            }

            //いま追加されているタスクをすべて表示する
            Debug.Log("task list:");
            Debug.Log("output start");
            foreach (string key in taskDictionary.Keys)
            {
                Debug.Log(key);
            }
            Debug.Log("output end");
            return;
        }

        stopList.Add(name);
    }

    public void StopAll()
    {
        foreach(string key in taskDictionary.Keys)
        {
            stopList.Add(key);
        }
    }
}

public interface IMyTask
{
    BehaviourStatus Update();
    void Stop();
}


public class MyTask : IMyTask
{
    public Func<BehaviourStatus> OnUpdate;
    public Action OnExit;

    public MyTask(Func<BehaviourStatus> onUpdate, Action onExit = null)
    {
        OnUpdate = onUpdate;
        OnExit = onExit;
    }

    public BehaviourStatus Update()
    {
        //アップデートは必須なのでnullチェックはしない
        return OnUpdate.Invoke();
    }

    public void Stop()
    {
        if (OnExit != null) OnExit.Invoke();
    }
}

