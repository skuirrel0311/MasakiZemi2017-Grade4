﻿using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MyTriggerBox : MyTrigger
{
    BoxCollider m_collider;
    Vector3 boxSize;
    Vector3 offset;

    void Start()
    {
        m_collider = GetComponent<BoxCollider>();
        Vector3 m_size = transform.lossyScale;
        m_collider.isTrigger = true;
        boxSize = m_collider.size * 0.5f;
        boxSize.x *= m_size.x;
        boxSize.y *= m_size.y;
        boxSize.z *= m_size.z;
        offset = m_collider.center;
        offset.x *= m_size.x;
        offset.y *= m_size.y;
        offset.z *= m_size.z;

#if !UNITY_EDITOR
        //実機では見えている必要はないので削除
        DestroyImmediate(m_collider);
#endif
    }

    /// <summary>
    /// 渡されたobjが自身のトリガーにヒットしているかを返します
    /// </summary>
    public override bool Intersect(GameObject obj, LayerMask layer)
    {
        cols = Physics.OverlapBox(m_transform.position + offset, boxSize, m_transform.rotation, layer);

        return Intersect(obj);
    }

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        var oldColor = UnityEditor.Handles.color;
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireCube(m_transform.position + offset,Vector3.one * 0.1f);
        UnityEditor.Handles.color = oldColor;
#endif
    }
}
