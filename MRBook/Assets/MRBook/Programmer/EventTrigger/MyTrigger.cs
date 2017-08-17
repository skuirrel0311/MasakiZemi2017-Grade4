using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyTrigger : MonoBehaviour
{
    protected Collider[] cols;
    protected Transform m_transform;

    protected virtual void Awake()
    {
        m_transform = transform;
    }
    public abstract bool Intersect(GameObject obj, LayerMask layer);

    protected bool Intersect(GameObject obj)
    {
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.Equals(obj)) return true;
        }
        return false;
    }
}
