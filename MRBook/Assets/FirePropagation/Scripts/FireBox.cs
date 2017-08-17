/* Copyright (c) 2016-2017 Lewis Ward
// Fire Propagation System
// author: Lewis Ward
// date  : 03/02/2017
*/
using UnityEngine;
using System.Collections;

public class FireBox {
    public Vector3 m_radius = new Vector3(1, 1, 1);
    private Vector3 m_position;
    private string m_terrainName = "Terrain";
    private Collider[] m_overlapOjects = new Collider[10];
    public Vector3 radius
    {
        get { return m_radius; }
        set { m_radius = value; }
    }

    public void init(Vector3 position, string terrianName)
    {
        m_position = position;
        m_terrainName = terrianName;
    }

    public void detectionTest()
    {
        Physics.OverlapBoxNonAlloc(m_position, m_radius, m_overlapOjects);

        // active FireChain if the collided GameObject has one
        for (int i = 0; i < 10; i++)
            if (m_overlapOjects[i] != null)
                if (m_overlapOjects[i].name != m_terrainName)
                    activePresentFireChains(m_overlapOjects[i]);
    }

    bool activePresentFireChains(Collider gameObject)
    {
        FireNodeChain chain = gameObject.GetComponent<FireNodeChain>();

        if (chain != null)
        {
            chain.startFire(m_position);
            return true;
        }

        return false;
    }
}
