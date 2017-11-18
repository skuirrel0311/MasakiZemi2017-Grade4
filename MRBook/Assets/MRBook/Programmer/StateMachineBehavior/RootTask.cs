using System.Collections.Generic;
using UnityEngine;

//一番上に居るタスク　ステート内のデータを保持している
public class RootTask : Sequence
{
    public static RootTask I = null;

    //[System.NonSerialized]
    public List<BaseStateMachineBehaviour> taskList = new List<BaseStateMachineBehaviour>();

    AnimatorBakedData bakeData;

    Dictionary<int, AnimatorBakedData.MyAnimatorState> dictionary;
    Dictionary<int, AnimatorBakedData.MyAnimatorState> Dictionary
    {
        get
        {
            if (dictionary != null) return dictionary;

            dictionary = new Dictionary<int, AnimatorBakedData.MyAnimatorState>();

            for (int i = 0; i < bakeData.hashList.Count; i++)
            {
                Debug.Log("add");
                dictionary.Add(bakeData.hashList[i], bakeData.stateList[i]);
            }

            return dictionary;
        }
    }

    protected override void OnStart()
    {
        I = this;
        m_animator.SetInteger("StateStatus", 0);
        bakeData = Resources.Load<AnimatorBakedData>("Data/" + m_animator.runtimeAnimatorController.name);
        //AnimatorBakedData.MyAnimatorState state;

        Debug.Log("full hash = " + m_stateInfo.fullPathHash);
        //Debug.Log("count = " + Dictionary.Count);

        foreach (int key in Dictionary.Keys)
        {
            Debug.Log("hash = " + key);
        }

        //taskList.AddRange(GetBehaviours(state));
        //子を持つタスクに子を格納してもらう
        for (int i = taskList.Count - 1; i >= 0; i--)
        {
            taskList[i].Init(i);
        }

        //復元しておく
        taskList.Clear();
        //taskList.AddRange(GetBehaviours(state));

        //base.OnStart();
    }

    BaseStateMachineBehaviour[] GetBehaviours(AnimatorBakedData.MyAnimatorState state)
    {
        return null;
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        m_animator.SetInteger("StateStatus", 1);
    }
}
