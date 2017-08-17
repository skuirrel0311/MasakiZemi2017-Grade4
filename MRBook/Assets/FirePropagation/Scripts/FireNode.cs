/* Copyright (c) 2016-2017 Lewis Ward
// Fire Propagation System
// author: Lewis Ward
// date  : 01/02/2017
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireNode : MonoBehaviour
{
    [Tooltip("Prefab to be used for the fire.")]
    public GameObject m_fire;
    [Tooltip("GameObjects with a FireNode script, any linked node will be heated up once this node is on fire.")]
    public List<GameObject> m_links = null;
    [Tooltip("The Hit Points of the cell, the higher the HP to slower the cell is to heat up and ignite.")]
    public float m_HP = 50.0f;
    [Tooltip("Amount of fuel in the cell.")]
    public float m_fuel = 50.0f;
    private float m_extinguishThreshold;
    private float m_combustionRateValue;
    private bool m_fireJustStarted = false;
    private bool m_isAlight = false;
    private bool m_extingushed = false;
    private bool m_clean = false;
    private FireVisualManager m_visualMgr = null;
    public GameObject flames { get { return m_fire; } }
    public bool isAlight { get { return m_isAlight; } }
    public bool fireJustStarted
    {
        get { return m_fireJustStarted; }
        set { m_fireJustStarted = value; }
    }
    public float HP
    {
        get { return m_HP; }
        set { m_HP = value; }
    }
    public float extinguishThreshold { get { return m_extinguishThreshold; } }


    // Use this for initialization
    void Start()
    {
        // if a tag was not set in the editor then fallback to slower why of finding the object
        try
        {
            GameObject manager = GameObject.FindWithTag("Fire");

            if (manager != null)
            {
                FireManager fireManager = manager.GetComponent<FireManager>();

                if (fireManager != null)
                {
                    m_extinguishThreshold = m_fuel * fireManager.visualExtinguishThreshold;
                    m_combustionRateValue = fireManager.nodeCombustionRate;
                }
                else
                {
                    m_extinguishThreshold = m_fuel;
                    m_combustionRateValue = 1.0f;
                }
            }
        }
        catch
        {
            // get the terrain from the fire manager
            FireManager fireManager = FindObjectOfType<FireManager>();

            if (fireManager != null)
            {
                m_extinguishThreshold = m_fuel * fireManager.visualExtinguishThreshold;
                m_combustionRateValue = fireManager.nodeCombustionRate;
            }
            else
            {
                m_extinguishThreshold = m_fuel;
                m_combustionRateValue = 1.0f;
            }
        }
    }

    // kills the attached child particle systems
    private void killFlames()
    {
        Destroy(m_fire);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_HP <= 0.0f && !m_isAlight)
            m_fireJustStarted = true;

        ingition();
        combustion();
    }

    // has this node ran out of fire fuel, returns true if it has
    public bool nodeConsumed()
    {
        if (m_clean == true)
            return true;
        else
            return false;
    }

    // force this script to update
    public void forceUpdate()
    {
        Update();
    }

    public void instantiateFire(Vector3 position, GameObject Fire)
    {
        if (m_fireJustStarted)
        {
            m_fire = (GameObject)Instantiate(Fire, position, new Quaternion());

            // should be set after fire extinguished
            m_isAlight = true;
        }
    }

    void ingition()
    {
        if (m_fireJustStarted && !m_extingushed)
        {
            instantiateFire(transform.position, m_fire);
            m_fireJustStarted = false;

            getFireManager();

            if(m_visualMgr != null)
                m_visualMgr.setIgnitionState();
        }
    }

    void combustion()
    {
        if (m_isAlight)
        {
            m_fireJustStarted = false;

            m_fuel -= m_combustionRateValue * Time.deltaTime;

            if (m_fuel < m_extinguishThreshold)
            {
                // should be valid as getFireManager() called in ingition before this function is called
                if (m_visualMgr != null)
                    m_visualMgr.setExtingushState();
            }

            if (m_fuel <= 0.0f)
            {
                m_isAlight = false;
                m_extingushed = true;

                killFlames();
                m_clean = true;
            }
        }
    }

    void getFireManager()
    {
        if (m_visualMgr == null)
            m_visualMgr = m_fire.GetComponent<FireVisualManager>();
    }
}
