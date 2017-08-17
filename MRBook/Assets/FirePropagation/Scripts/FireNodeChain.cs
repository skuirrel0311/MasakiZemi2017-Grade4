/* Copyright (c) 2016-2017 Lewis Ward
// Fire Propagation System
// author: Lewis Ward
// date  : 03/02/2017
*/
using UnityEngine;
using System.Collections;

public class FireNodeChain : MonoBehaviour {
    [Tooltip("Higher the value, quick the fire ignites fuel")]
    public float m_firePropagationSpeed = 20.0f;
    [Tooltip("Nodes within this chain, should have all nodes so fires start correctly")]
    public FireNode[] m_fireNodes = null;
    [Tooltip("Enable if GameObject should be destroyed once all nodes been set on fire, do not enable for trees!")]
    public bool m_destroyAfterFire = false;
    [Tooltip("Enable if GameObject should be replaced with another mesh once all nodes have been set on fire")]
    public bool m_replaceAfterFire = false;
    [Tooltip("The GameObject that this object should be replaced with")]
    public GameObject m_replacementMesh;
    private float m_combustionRateValue = 1.0f;
    private bool m_destroyedAlready = false;
    private bool m_replacedAlready = false;
    private bool m_validChain = true;

    public float propagationSpeed
    {
        get { return m_firePropagationSpeed; }
        set { m_firePropagationSpeed = value; }
    }

    // Use this for initialization
    void Start () {

        try
        {
            GameObject manager = GameObject.FindWithTag("Fire");

            if (manager != null)
            {
                FireManager fireManager = manager.GetComponent<FireManager>();
                if (fireManager != null)
                    m_combustionRateValue = fireManager.nodeCombustionRate;
            }
        }
        catch
        {
            // get the terrain from the fire manager
            FireManager fireManager = FindObjectOfType<FireManager>();
            if (fireManager != null)
                m_combustionRateValue = fireManager.nodeCombustionRate;

            Debug.LogWarning("No 'Fire' tag set, looking for Fire Manager.");
        }

        // make sure that all nodes in the chain have been assigned
        for (int i = 0; i < m_fireNodes.Length; i++)
        {
            if (m_fireNodes[i] == null)
            {
                Debug.LogError("Fire Node Chain on " + gameObject.GetComponentInParent<Transform>().name + " has missing Fire Nodes!");
                m_validChain = false;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (m_validChain)
        {
            propagateFire();

            if (m_destroyAfterFire && !m_destroyedAlready)
                destroyAfterFire();

            if (m_replaceAfterFire && !m_replacedAlready)
                replaceAfterFire();
        }
    }

    // creates fire partlice systems on the node positions
    void propagateFire()
    {
        // propagate fire over the nodes, based on which nodes are on fire, not on fire and if they have been on fire before
        for (int i = 0; i < m_fireNodes.Length; i++)
        {
            if (m_fireNodes[i].isAlight)
            {
                for (int child = 0; child < m_fireNodes[i].m_links.Count; child++)
                {
                    if (m_fireNodes[i].m_links[child].GetComponent<FireNode>().HP > 0.0f)
                    {
                        m_fireNodes[i].m_links[child].GetComponent<FireNode>().HP -= m_firePropagationSpeed * Time.deltaTime;
                    }
                }
            }

            m_fireNodes[i].forceUpdate();
        }
    }

    // find the closest node to the fire as set it alight
    public void startFire(Vector3 firePoisition)
    {
        float shortestDistance = float.MaxValue;
        int shortestIndex = 0;
        for (int i = 0; i < m_fireNodes.Length; i++)
        {
            float distance = Vector3.Distance(m_fireNodes[i].GetComponent<Transform>().position, firePoisition);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                shortestIndex = i;
            }
        }

        m_fireNodes[shortestIndex].HP -= m_combustionRateValue * Time.deltaTime;
    }

    void destroyAfterFire()
    {
        bool allBurnt = false;

        // check all nodes have had they fuel used up
        for (int i = 0; i < m_fireNodes.Length; i++)
        {
            if (m_fireNodes[i].m_fuel <= 0.0f)
            {
                // got to the end, all have ran out of fuel
                if (i == m_fireNodes.Length - 1)
                    allBurnt = true;

                // need to check next node
                continue;
            }
            else // still have fuel
            {
                break;
            }
        }

        // if so, delete the gameoject
        if (allBurnt)
        {
            Destroy(gameObject);
            m_destroyedAlready = true;
        }
    }

    void replaceAfterFire()
    {
        bool allBurnt = false;

        // check all nodes have had they fuel used up
        for (int i = 0; i < m_fireNodes.Length; i++)
        {
            if (m_fireNodes[i].nodeConsumed() == true)
            {
                // got to the end, all have ran out of fuel
                if (i == m_fireNodes.Length - 1)
                    allBurnt = true;

                // need to check next node
                continue;
            }
            else
            {
                break;
            }
        }

        // if so, delete the gameoject and replace it
        if (allBurnt && m_replacementMesh != null)
        {
            if (m_replacementMesh != null)
            {
                Transform trans = gameObject.transform;
                Destroy(gameObject);
                Instantiate(m_replacementMesh, trans.position, trans.rotation);
            }
            else
            {
                Debug.Log("Failed to replace the gameobject");
            }


            m_replacedAlready = true;
        }
    }
}
