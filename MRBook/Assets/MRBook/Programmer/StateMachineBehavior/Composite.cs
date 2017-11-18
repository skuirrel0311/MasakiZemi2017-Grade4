using UnityEngine;

//対応したEndPointまでを子に持ちます
public class Composite : ParentTask
{
    public override TaskType GetTaskType { get { return TaskType.Composite; } }

    protected override void SetChildTask()
    {
        Debug.Log("call set child");
        //自身の1つ後ろから見ていく
        for (int i = selfIndex + 1; i < RootTask.I.taskList.Count; i++)
        {
            //一つでも子がある
            HasChild = true;
            BaseStateMachineBehaviour child = RootTask.I.taskList[i];
            child.SetParent(this);
            Debug.Log("add child");
            childTask.Add(child);

            if (child.GetTaskType == TaskType.EndPoint) break;
        }

        //子に追加したタスクは親から削除しておく
        for (int i = 0; i < childTask.Count; i++)
        {
            RootTask.I.taskList.Remove(childTask[i]);
        }

        //EndPointはいらないので子タスクから削除しておく
        childTask.Remove(childTask[childTask.Count - 1]);
    }
}
