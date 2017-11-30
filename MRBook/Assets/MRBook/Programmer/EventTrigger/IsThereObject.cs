using UnityEngine;

public class IsThereObject : MyEventTrigger
{
    MyTrigger trigger;

    [SerializeField]
    LayerMask layer = 1 << 8;

    [SerializeField]
    protected string objName = "";
    HoloObject targetObject;

    public void Awake()
    {
        trigger = GetComponent<MyTrigger>();

        if (trigger == null)
        {
            Debug.LogError(name + " don't have MyTrigger");
        }
    }

    public override void SetFlag()
    {
        if (trigger == null) FlagManager.I.SetFlag(flagName, this, false);
        if(targetObject == null && !TryGetTargetObject(out targetObject)) return;

        bool isHit = trigger.Intersect(targetObject.gameObject, layer);
        //フラグマネージャーに結果を保存する
        FlagManager.I.SetFlag(flagName, this, isHit);
    }

    bool TryGetTargetObject(out HoloObject obj)
    {
        obj = ActorManager.I.GetObject(objName);
        return obj != null;
    }
}
