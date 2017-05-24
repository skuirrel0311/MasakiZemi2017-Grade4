using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class TestSceneManager : BaseManager<TestSceneManager>
{
    BehaviorTree tree;

    protected override void Start()
    {
        base.Start();
        tree = GetComponent<BehaviorTree>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            tree.EnableBehavior();
    }
}
