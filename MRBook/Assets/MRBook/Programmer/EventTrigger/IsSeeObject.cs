using UnityEngine;

public class IsSeeObject : MyEventTrigger
{
    //視点
    [SerializeField]
    Transform eye = null;

    public LayerMask ignoreLayerMask;

    public override void SetFlag()
    {
        Vector3 direction = targetObject.transform.position - eye.position;

        //まず角度
        float angle = Vector3.Angle(transform.forward, direction);
        if (angle > 30.0f)
        {
            FlagManager.I.SetFlag(flagName,this, false);
            return;
        }

        //障害物はないか
        Ray ray = new Ray(eye.position, Vector3.Normalize(direction));
        RaycastHit[] cols = Physics.RaycastAll(ray, direction.magnitude,~ignoreLayerMask);

        for(int i = 0;i< cols.Length;i++)
        {
            //自身は省く
            if (cols[i].transform.gameObject.Equals(transform.parent.gameObject)) continue;

            FlagManager.I.SetFlag(flagName, this, cols[i].transform.gameObject.Equals(targetObject));
            return;
        }

        //障害物がなかった
        FlagManager.I.SetFlag(flagName, this, true);
    }
}
