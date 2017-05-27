using UnityEngine;

public class MyTriggerBox : MonoBehaviour
{
    Vector3 boxSize;

    void Start()
    {
        BoxCollider m_collider = GetComponent<BoxCollider>();
        Vector3 m_size = transform.lossyScale;
        boxSize = m_collider.size * 0.5f;
        boxSize.x *= m_size.x;
        boxSize.y *= m_size.y;
        boxSize.z *= m_size.z;
    }

    /// <summary>
    /// 渡されたobjが自身のトリガーにヒットしているかを返します
    /// </summary>
    public bool Intersect(GameObject obj, LayerMask layer)
    {
        Collider[] cols = Physics.OverlapBox(transform.position, boxSize, Quaternion.identity, layer);
        for(int i = 0;i < cols.Length;i++)
        {
            if (cols[i].gameObject.Equals(obj)) return true;            
        }
        return false;
    }
}
