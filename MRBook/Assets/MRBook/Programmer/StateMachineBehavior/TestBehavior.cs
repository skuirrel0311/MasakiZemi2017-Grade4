using UnityEngine;

public class TestBehavior : BaseStateMachineBehaviour
{
    public string message;

    protected override void OnStart()
    {
        base.OnStart();
        NotificationManager.I.ShowMessage(message, 5.0f);
    }
}
