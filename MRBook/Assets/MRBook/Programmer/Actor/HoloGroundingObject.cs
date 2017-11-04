using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 離した時に地面に落ちるオブジェクト
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class HoloGroundingObject : HoloMovableObject
{
    public override bool IsGrounding { get { return true; } }

    public NavMeshAgent m_agent{ get; private set; }

    Coroutine fallCoroutine;

    protected override void Awake()
    {
        base.Awake();
        m_agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 地面に落とす
    /// </summary>
    public override void Fall()
    {
        if(fallCoroutine != null)
        {
            StopCoroutine(fallCoroutine);
        }
        fallCoroutine = StartCoroutine(Falling());
    }

    //0.1ずつ落ちて地面が近かったらAgentを有効化する
    IEnumerator Falling()
    {
        NavMeshHit hit;
        while (true)
        {
            //0.1ずつ下を探す
            transform.position += Vector3.down * 0.1f;

            //todo : 絵本よりも下に行った場合はやばいのでなにか対応が必要
            
            if(NavMesh.SamplePosition(transform.position, out hit, m_agent.height, NavMesh.AllAreas))
            {
                break;
            }

            yield return null;
        }

        m_agent.enabled = true;
        fallCoroutine = null;
    }
}
