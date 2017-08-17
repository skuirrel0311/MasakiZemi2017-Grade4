using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestObject : MonoBehaviour
{
    NavMeshAgent m_agent;
    [SerializeField]
    Transform targetPoint = null;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.SetDestination(targetPoint.position);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_agent.isStopped = true;
        }
    }
}
