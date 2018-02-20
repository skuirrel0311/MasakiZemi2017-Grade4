using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasItem : MyEventTrigger
{
    [SerializeField]
    string targetItemName = "";

    [SerializeField]
    ActorName ownerCharacterName;

    HoloObject owner;

    public override void SetFlag()
    {
        SetOwnerCharacter();
        FlagManager.I.SetFlag(flagName, this, OwnerHasItem(targetItemName));
    }

    protected void SetOwnerCharacter()
    {
        owner = ActorManager.I.GetCharacter(ownerCharacterName);
    }

    protected bool OwnerHasItem(string itemName)
    {
        HoloItem item;
        if (!TryGetHoloItem(itemName, out item)) return false;

        if (item.owner == null) return false;

        return item.owner.Equals(owner);
    }

    bool TryGetHoloItem(string itemName, out HoloItem item)
    {
        item = null;
        HoloObject obj = ActorManager.I.GetObject(itemName);

        if (obj == null) return false;
        
        item = (HoloItem)obj;
        return item != null; ;
    }
}
