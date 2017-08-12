using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MyTriggerSphere : MyTrigger
{
    SphereCollider m_collider;
    float radius;

    void Start()
    {
        m_collider = GetComponent<SphereCollider>();
        Vector3 m_size = transform.lossyScale;

        float temp = Math.Max(m_size.x, Math.Max(m_size.y, m_size.z));
        radius = m_collider.radius * temp;

#if !UNITY_EDITOR
        //実機では見えている必要はないので削除
        DestroyImmediate(m_collider);
#endif
    }

    public override bool Intersect(GameObject obj, LayerMask layer)
    {
        cols = Physics.OverlapSphere(m_transform.position, radius, layer);
        return Intersect(obj);
    }
}
