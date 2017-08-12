using UnityEngine;

public class IsThereObject : MyEventTrigger
{
    MyTrigger trigger;

    [SerializeField]
    LayerMask layer;

    public void Start()
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

        bool isHit = trigger.Intersect(targetObject, layer);
        //フラグマネージャーに結果を保存する
        FlagManager.I.SetFlag(flagName, this, isHit);
    }
}
