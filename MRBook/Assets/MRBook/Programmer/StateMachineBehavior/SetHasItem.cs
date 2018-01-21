using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 名前がわからないアイテムを渡すのに使う
/// </summary>
public class SetHasItem : BaseStateMachineBehaviour
{
    public enum ParentType { HoloObject, Page }

    //～から
    [SerializeField]
    ActorName fromCharacterName = ActorName.Urashima;
    //～へ
    [SerializeField]
    ActorName toCharacterName = ActorName.Urashima;

    protected override void OnStart()
    {
        base.OnStart();

        HoloCharacter fromCharacter = ActorManager.I.GetCharacter(fromCharacterName);

        if (fromCharacter == null)
        {
            return;
        }

        HoloCharacter toCharacter = ActorManager.I.GetCharacter(toCharacterName);

        if(toCharacter == null)
        {
            return;
        }

        CharacterItemSaucer fromCharacterItemSaucer = (CharacterItemSaucer)fromCharacter.ItemSaucer;
        CharacterItemSaucer toCharacterItemSaucer = (CharacterItemSaucer)toCharacter.ItemSaucer;

        HoloItem rightHandItem = fromCharacterItemSaucer.RightHandItem;
        HoloItem leftHandItem = fromCharacterItemSaucer.LeftHandItem;

        //right,leftどっちもnullじゃないと面白いことになる
        if (rightHandItem != null)
        {
            fromCharacterItemSaucer.DumpItem(rightHandItem, false);
            toCharacterItemSaucer.SetItem(rightHandItem, false, false);
        }
        if (leftHandItem != null)
        {
            fromCharacterItemSaucer.DumpItem(leftHandItem, false);
            toCharacterItemSaucer.SetItem(leftHandItem, false, false);
        }

    }
}
