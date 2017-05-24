using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class MyEvent : Action
{
    public string text;
    //public TextMesh text;

    public override TaskStatus OnUpdate()
    {
        //todo:キャラクターが動いたりする？
        //text.gameObject.SetActive(true);
        Debug.Log(text);
        return TaskStatus.Success;
    }
}
