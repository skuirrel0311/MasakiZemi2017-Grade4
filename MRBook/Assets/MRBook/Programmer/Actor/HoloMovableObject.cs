using UnityEngine;
using UnityEngine.AI;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// 動く可能性のあるオブジェクト（位置のリセットが必要だということ）
/// </summary>
[HideInInspector]
public class HoloMovableObject : HoloObject
{

    //void SetSphreCastRadius()
    //{
    //    if (!IsFloating)
    //    {
    //        SphereCastRadius = m_agent.radius;
    //    }
    //    else
    //    {
    //        float colSize = Mathf.Max(m_collider.size.x, m_collider.size.z);
    //        float scale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
    //        SphereCastRadius = colSize * scale;
    //    }
    //}
}
