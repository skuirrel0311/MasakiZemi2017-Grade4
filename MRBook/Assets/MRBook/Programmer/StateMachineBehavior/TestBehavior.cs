using UnityEngine;

public class TestBehavior : StateMachineBehaviour
{
    public string message;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NotificationManager.I.ShowMessage(message, 5.0f);
    }
}
