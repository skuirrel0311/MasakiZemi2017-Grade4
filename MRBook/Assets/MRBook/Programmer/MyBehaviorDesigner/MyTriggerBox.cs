using UnityEngine;

public class MyTriggerBox : MonoBehaviour
{
    Vector3 boxSize;

    void Start()
    {
        BoxCollider m_Collider = GetComponent<BoxCollider>();
        boxSize = m_Collider.size * 0.5f;
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
