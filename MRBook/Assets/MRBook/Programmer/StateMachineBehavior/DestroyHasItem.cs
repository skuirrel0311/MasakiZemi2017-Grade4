using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// dumpItemしてからdestroyObjectするやつ
/// </summary>
public class DestroyHasItem : BaseStateMachineBehaviour
{
    [SerializeField]
    ActorName actorName = ActorName.Urashima;

    protected override void OnStart()
    {
        base.OnStart();

        HoloCharacter character = ActorManager.I.GetCharacter(actorName);

        if (character == null)
        {
            return;
        }
        CharacterItemSaucer characterItemSaucer = (CharacterItemSaucer)character.ItemSaucer;

        characterItemSaucer.DumpItem();

    }
}
