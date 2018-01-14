using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DisableAgent : BaseStateMachineBehaviour
{
    [SerializeField]
    string targetName = "";

    protected override void OnStart()
    {
        base.OnStart();

        HoloObject obj = ActorManager.I.GetObject(targetName);

        if (obj == null) return;

        NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();

        if(agent == null)
        {
            Debug.LogError("agent is don't attach");
            return;
        }

        agent.enabled = false;

        MainSceneManager.I.OnReset += () => agent.enabled = true;
    }
}
