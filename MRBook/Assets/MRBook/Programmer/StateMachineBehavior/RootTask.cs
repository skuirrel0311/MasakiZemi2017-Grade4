using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一番上に居るタスク　ステート内のデータを保持している
public class RootTask : BaseStateMachineBehaviour
{
    public static RootTask I = null;

    [System.NonSerialized]
    public List<BaseStateMachineBehaviour> taskList = new List<BaseStateMachineBehaviour>();

    [System.NonSerialized]
    public Animator m_animator;

    public override void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        I = this;
        m_animator = animator;

        taskList.AddRange(animator.GetBehaviours<BaseStateMachineBehaviour>());

        //子を持つタスクに子を格納してもらう
        for (int i = taskList.Count - 1; i > 0; i--)
        {
            if (!taskList[i].HasChild) continue;
            taskList[i].InitChildTask(i);
        }
        
        //復元しておく
        taskList.Clear();
        taskList.AddRange(animator.GetBehaviours<BaseStateMachineBehaviour>());

        for (int i = taskList.Count - 1; i > 0; i--)
        {
            if (taskList[i].GetTaskType == TaskType.EndPoint)
            {
                taskList.Remove(taskList[i]);
            }
        }

        taskList.Remove(this);
    }

    public void OnEndTask()
    {
        bool isAllEnd = IsAllTaskEnd();
        if (isAllEnd) m_animator.SetTrigger("EndState");
    }

    bool IsAllTaskEnd()
    {
        //後ろから見たほうが効率が良い
        for (int i = taskList.Count - 1; i > 0; i--)
        {
            //一つでもisEndがfalseだったら終わっていない
            if (!taskList[i].isEnd) return false;
        }
        return true;
    }
}
