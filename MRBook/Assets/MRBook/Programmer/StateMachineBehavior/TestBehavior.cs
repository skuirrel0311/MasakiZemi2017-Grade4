using UnityEngine;

public class TestBehavior : BaseStateMachineBehaviour
{
    public string message;

    public override void OnStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStart(animator, stateInfo, layerIndex);
        NotificationManager.I.ShowMessage(message, 5.0f);
    }
}
