﻿using System.Collections.Generic;
using UnityEngine;

//一番上に居るタスク　ステート内のデータを保持している
public class RootTask : Sequence
{
    public static RootTask I = null;

    [System.NonSerialized]
    public List<BaseStateMachineBehaviour> taskList = new List<BaseStateMachineBehaviour>();

    protected override void OnStart()
    {
        Debug.Log("on start root");
        I = this;
        m_animator.SetInteger("StateStatus", 0);

        taskList.AddRange(GetBehaviours());

        //子を持つタスクに子を格納してもらう
        for (int i = taskList.Count - 1; i >= 0; i--)
        {
            taskList[i].Init(i,m_animator, m_stateInfo, m_layerIndex);
        }

        //復元しておく
        taskList.Clear();
        taskList.AddRange(GetBehaviours());
        Debug.Log("taskList.Count = " + taskList.Count);
        for (int i = 0; i < taskList.Count; i++)
        {
            Debug.Log("behaviour = " + taskList[i].ToString());
        }
        base.OnStart();
    }

    BaseStateMachineBehaviour[] GetBehaviours()
    {
        AnimatorBakedData.MyAnimatorState state;

        if(!StateMachineManager.I.AnimatorStateDictionary.TryGetValue(m_stateInfo.fullPathHash, out state)) return null;
        BaseStateMachineBehaviour[] behaviours = new BaseStateMachineBehaviour[state.length];
        BaseStateMachineBehaviour[] allBehaviours = m_animator.GetBehaviours<BaseStateMachineBehaviour>();
        int index = state.index;
        for(int i = 0;i< state.length;i++)
        {
            behaviours[i] = allBehaviours[i + index];
        }

        return behaviours;
    }

    protected override void OnEnd()
    {
        isActive = false;
        int state = CurrentStatus == BehaviourStatus.Success ? 1 : -1;
        m_animator.SetInteger("StateStatus", state);
        Debug.Log("on end root");

        taskList.Clear();
    }
}
