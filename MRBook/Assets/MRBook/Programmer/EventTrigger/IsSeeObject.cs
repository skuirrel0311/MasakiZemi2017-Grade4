﻿using UnityEngine;

/// <summary>
/// targetObjectをownerがeyeから見えるか？
/// </summary>
public class IsSeeObject : MyEventTrigger
{
    /// <summary>
    /// 誰が
    /// </summary>
    [SerializeField]
    ActorName ownerName = ActorName.Urashima;
    HoloCharacter character;

    /// <summary>
    /// 誰を
    /// </summary>
    [SerializeField]
    protected string targetObjName;
    HoloObject targetObject;

    /// <summary>
    /// どこから
    /// </summary>
    [SerializeField]
    Transform eye = null;

    [SerializeField]
    LayerMask ignoreLayerMask = 1 << 2;
    Ray ray;
    
    public override void SetFlag()
    {
        character = ActorManager.I.GetCharacter(ownerName);
        targetObject = ActorManager.I.GetObject(targetObjName);
        Vector3 direction = targetObject.transform.position - eye.position;

        //まず角度
        float angle = Vector3.Angle(transform.forward, direction);
        if (angle > 45.0f)
        {
            Debug.Log(flagName + " out angle");
            FlagManager.I.SetFlag(flagName, this, false);
            return;
        }

        //障害物はないか
        ray = new Ray(eye.position, Vector3.Normalize(direction));
        float maxDistance = direction.magnitude;
        float currentDistance = maxDistance;
        RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, ~ignoreLayerMask);
        GameObject hit = null;

        for (int i = 0; i < hits.Length; i++)
        {
            //自身は省く
            if (character.Equals(hits[i].transform.gameObject)) continue;

            //自身以外に当たった
            if (hits[i].distance < currentDistance)
            {
                hit = hits[i].transform.gameObject;
            }
        }

        Debug.Log(ownerName.ToString() + " is see " + (hit != null ? hit.name : "null"));
        //targetObjectとhitが同じだったら障害物はなかったということ
        FlagManager.I.SetFlag(flagName, this, targetObject.Equals(hit));
    }
}
