using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem : BaseStateMachineBehaviour
{
    [SerializeField]
    ActorName actorName = ActorName.Urashima;

    [SerializeField]
    string itemName = "";

    protected override void OnStart()
    {
        base.OnStart();
        HoloCharacter character = ActorManager.I.GetCharacter(actorName);

        if (character == null)
        {
            Debug.Log("not found " + actorName.ToString());
            return;
        }

        HoloObject itemObj = ActorManager.I.GetObject(itemName);
        if (itemObj == null)
        {
            Debug.Log("not found " + itemName);
            return;
        }

        HoloItem item;
        try
        {
            item = (HoloItem)itemObj;
        }
        catch
        {
            Debug.Log(itemName + " is not item");
            return;
        }

        if (item.owner != null)
        {
            //誰かにもたれている場合は離してもらう
            item.owner.ItemSaucer.DumpItem(item, false);
        }
        character.ItemSaucer.SetItem(item, false);

    }
}
