using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSceneManager : BaseManager<TutorialSceneManager>
{
    [SerializeField]
    HoloPuppet urashima = null;

    protected override void Start()
    {
        base.Start();
        Debug.Log("call init");
        urashima.Init();
    }
}
