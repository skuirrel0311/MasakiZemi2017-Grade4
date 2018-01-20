using UnityEngine;

public class TestBehavior : BaseStateMachineBehaviour
{
    [Multiline]
    public string message;

    [SerializeField]
    float lifeTime = 5.0f;

    protected override void OnStart()
    {
        base.OnStart();
        MissionTextController.I.AddText(message);
        NotificationManager.I.ShowMessage(message, lifeTime);
    }
}
