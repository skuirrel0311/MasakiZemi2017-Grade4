using System.Collections.Generic;
using UnityEngine;

public class Composite : HaveChildTask
{
    public override TaskType GetTaskType { get { return TaskType.Composite; } }

    public override void InitChildTask(int selfIndex)
    {
        int i = 0;
        //自身の1つ後ろから見ていく
        for (i = selfIndex + 1; i < RootTask.I.taskList.Count; i++)
        {
            childTask.Add(RootTask.I.taskList[i]);
            if (RootTask.I.taskList[i].GetTaskType == TaskType.EndPoint)
            {
                break;
            }
        }

        //子に追加したタスクは親から削除しておく
        for(i = 0;i< childTask.Count;i++)
        {
            childTask[i].isActive = false;
            RootTask.I.taskList.Remove(childTask[i]);
        }

        //EndPointはいらないので子タスクから削除しておく
        childTask.Remove(childTask[childTask.Count - 1]);
    }
}
