using UnityEngine;

//一番簡単なタスク
public class TestLog : BaseStateMachineBehaviour
{
    [SerializeField]
    public string text = "";

    protected override void OnStart()
    {
        base.OnStart();
        Debug.Log(text);
    }
}
