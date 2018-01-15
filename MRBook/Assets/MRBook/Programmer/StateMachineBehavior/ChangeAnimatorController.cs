using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimatorController : BaseStateMachineBehaviour
{
    [SerializeField]
    ActorName characterName = ActorName.Urashima;
    [SerializeField]
    string animatorControllerName = "";

    protected override void OnStart()
    {
        base.OnStart();

        HoloCharacter character = ActorManager.I.GetCharacter(characterName);

        if (character == null) return;

        RuntimeAnimatorController animatorController = MyAssetStore.I.GetAsset<RuntimeAnimatorController>(animatorControllerName, "AnimatorControllers/");

        if(animatorController == null)
        {
            Debug.Log("not found " + animatorControllerName);
            return;
        }

        RuntimeAnimatorController oldAnimatorController = character.m_animator.runtimeAnimatorController;
        Debug.Log("change animator controller");
        character.m_animator.runtimeAnimatorController = animatorController;

        MainSceneManager.I.OnReset += () => character.m_animator.runtimeAnimatorController = oldAnimatorController;
    }
}
