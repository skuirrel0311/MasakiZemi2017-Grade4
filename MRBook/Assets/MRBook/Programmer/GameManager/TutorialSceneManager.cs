using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSceneManager : BaseManager<TutorialSceneManager>
{
    [SerializeField]
    HoloPuppet urashima = null;

    [SerializeField]
    HoloPuppet urashima2 = null;

    protected override void Start()
    {
        base.Start();
        urashima.Init();
        urashima2.Init();
    }
}
