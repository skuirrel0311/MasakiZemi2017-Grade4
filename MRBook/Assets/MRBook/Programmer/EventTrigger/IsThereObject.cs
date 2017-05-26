using UnityEngine;

public class IsThereObject : MyEventTrigger
{
    [SerializeField]
    GameObject targetObject = null;
    [SerializeField]
    LayerMask layer;

    MyTriggerBox trigger;

    public void Start()
    {
        trigger = GetComponent<MyTriggerBox>();
    }

    bool isHitObject()
    {
        if (trigger == null) return false;

        bool isHit = trigger.Intersect(targetObject, layer);
        //フラグマネージャーに結果を保存する
        FlagManager.I.SetFlag(flagName, isHit);

        return isHit;
    }

    public override void SetFlag()
    {
        if (trigger == null) FlagManager.I.SetFlag(flagName, false);

        bool isHit = trigger.Intersect(targetObject, layer);
        //フラグマネージャーに結果を保存する
        FlagManager.I.SetFlag(flagName, isHit);
    }
}
