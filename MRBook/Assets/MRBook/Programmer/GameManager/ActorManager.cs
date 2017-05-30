using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : BaseManager<ActorManager>
{
    [SerializeField]
    List<GameObject> actorList = new List<GameObject>();
    [SerializeField]
    List<Transform> targetList = new List<Transform>();

    protected override void Start()
    {
        base.Start();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < temp.Length; i++)
        {
            targetList.Add(temp[i].transform);
        }
    }

    public GameObject GetActor(string name)
    {
        for(int i = 0;i< actorList.Count;i++)
        {
            if (actorList[i].name == name) return actorList[i];
        }
        return null;
    }
    public Transform GetTarget(string name)
    {
        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i].name == name) return targetList[i];
        }
        return null;
    }
}
