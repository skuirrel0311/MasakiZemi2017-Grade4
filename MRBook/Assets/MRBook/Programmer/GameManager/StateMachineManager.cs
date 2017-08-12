using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineManager : BaseManager<StateMachineManager>
{
    public delegate void VoidEvent();

    Dictionary<string, VoidEvent> updateEventDictionary = new Dictionary<string, VoidEvent>();
    Dictionary<string, VoidEvent> stopEventDictionary = new Dictionary<string, VoidEvent>();

    /// <summary>
    /// キャラの名前とかをキーにするといいんじゃないかな？
    /// </summary>
    /// <param name="name"></param>
    /// <param name="e"></param>
    public void Add(string name, VoidEvent updateEvent, VoidEvent stopEvent)
    {
        updateEventDictionary.Add(name, updateEvent);
        stopEventDictionary.Add(name, stopEvent);
    }

    void Update()
    {
        foreach(string key in updateEventDictionary.Keys)
        {
            updateEventDictionary[key].Invoke();
        }
    }

    public void StopEvent(string name)
    {
        updateEventDictionary.Remove(name);
        stopEventDictionary[name].Invoke();
        stopEventDictionary.Remove(name);
    }
}
