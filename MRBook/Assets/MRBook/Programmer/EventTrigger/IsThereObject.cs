using UnityEngine;

public class IsThereObject : MyEventTrigger
{
    MyTriggerBox trigger;

    public void Start()
    {
        trigger = GetComponent<MyTriggerBox>();
    }

    public override void SetFlag()
    {
        if (trigger == null) FlagManager.I.SetFlag(flagName, false);

        bool isHit = trigger.Intersect(targetObject, layer);
        //フラグマネージャーに結果を保存する
        FlagManager.I.SetFlag(flagName, isHit);
    }
}
