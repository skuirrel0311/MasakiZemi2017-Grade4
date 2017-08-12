using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MyTriggerBox : MyTrigger
{
    BoxCollider m_collider;
    Vector3 boxSize;

    void Start()
    {
        m_collider = GetComponent<BoxCollider>();
        Vector3 m_size = transform.lossyScale;
        m_collider.isTrigger = true;
        boxSize = m_collider.size * 0.5f;
        boxSize.x *= m_size.x;
        boxSize.y *= m_size.y;
        boxSize.z *= m_size.z;
        
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
        cols = Physics.OverlapBox(m_transform.position, boxSize, m_transform.rotation, layer);

        return Intersect(obj);
    }
}
