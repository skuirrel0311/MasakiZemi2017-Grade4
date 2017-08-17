/* Copyright (c) 2016-2017 Lewis Ward
// Fire Propagation System
// author: Lewis Ward
// date  : 10/01/2017
*/
using UnityEngine;
using System.Collections;

public class FireCell : MonoBehaviour {
    private GameObject m_firePrefab; // prefab to be used for the fire
    private GameObject[] m_fires; // fire
    private FireBox m_fireBox = null; // used to detect collision with GameObjects that have colliders
    [Tooltip("The Hit Points of the cell, the higher the HP to slower the cell is to heat up and ignite.")]
    public float m_HP = 50.0f;
    [Tooltip("Amount of fuel in the cell.")]
    public float m_fuel = 60.0f;
    private float m_extinguishThreshold; // the level when the extingush stage particle systems should be run
    [Tooltip("The amount of ground moisture in the cell.")]
    public float m_moisture = 0.0f;
    private float m_ignitionTemperature = 1.0f;
    private float m_fireTemperature = 0.0f;
    private float m_combustionConstant = 1.0f;
    private float m_pIgnition = 0.0f;
    private bool m_instantiatedInCell = false;
    private bool m_temperatureModified = false;
    private bool m_fireProcessHappening = false;
    private bool m_fireJustStarted = false;
    private bool m_isAlight = false;
    private bool m_extinguish = false;
    private bool m_extingushed = false;
    private int m_iginitionCounter = 0;
    private Vector2[] m_firesPositions;

    public Vector3 position { get { return transform.position; } }
    public float fireTemperature
    {
        set { m_fireTemperature = value;  }
        get { return m_fireTemperature; }
    }
    public float ignitionTemperature
    {
        set { m_ignitionTemperature = value; }
        get { return m_ignitionTemperature; }
    }
    public bool temperatureModified
    {
        set { m_temperatureModified = value; }
        get { return m_temperatureModified; }
    }
    public float extinguishThreshold { get { return m_extinguishThreshold; } }
    public bool fireInCell { get { return m_instantiatedInCell; } }
    public bool isAlight { get { return m_isAlight; } }
    public bool fireProcessHappening { get { return m_fireProcessHappening; } }

	// Update is called once per frame
	void Update () {
        // if this cell is heating up but the fire moves away and can no longer heat it up, extingush this cell
        if (!m_fireJustStarted && !m_isAlight && m_fireProcessHappening && m_pIgnition == m_ignitionTemperature)
            m_iginitionCounter++;
        else
            m_iginitionCounter = 0;

        if (m_iginitionCounter > 300)
            m_extinguish = true;


        m_pIgnition = m_ignitionTemperature;
    }

    public void setupCell(bool alight, GameObject fire, CellData data, string terrainName, Vector2[] firesPositionsInCell)
    {
        m_isAlight = alight;
        m_firePrefab = fire;
        m_fires = new GameObject[firesPositionsInCell.Length];
        m_firesPositions = firesPositionsInCell;
        m_HP = data.HP;
        m_fuel = data.fuel;
        m_extinguishThreshold = data.fuel * data.threshold;
        m_moisture = data.moisture;
        m_fireBox = new FireBox();
        m_fireBox.init(transform.position, terrainName);
        float boxExtents = data.cellSize / 2.0f;
        m_fireBox.radius = new Vector3(boxExtents, boxExtents, boxExtents);
        m_combustionConstant = data.combustionValue;
        setInitialFireValues(data.airTemperature, data.propagationSpeed);
    }

    [System.Obsolete("Use setupCell() method.")]
    public void init(bool alight, GameObject fire, float airTemperature, float propagationSpeed, float HP, float fuel, float moisture, float threshold, float combustionValue, string terrainName, int firesInCell, Vector2[] firesPositionsInCell)
    {
        m_isAlight = alight;
        m_firePrefab = fire;
        m_fires = new GameObject[firesInCell];
        m_firesPositions = firesPositionsInCell;
        m_HP = HP;
        m_fuel = fuel;
        m_extinguishThreshold = fuel * threshold;
        m_moisture = moisture;
        m_fireBox = new FireBox(); 
        m_fireBox.init(transform.position, terrainName);
        m_fireBox.radius = new Vector3(0.5f, 0.5f, 0.5f);
        m_combustionConstant = combustionValue;
        setInitialFireValues(airTemperature, propagationSpeed);
    }

    public void delete()
    {
        // destory the fire in the cell now or after a delay
        if (m_instantiatedInCell && m_extingushed)
        {
            gameObject.SetActive(false);
            m_fireProcessHappening = false;
        }
        else if (m_instantiatedInCell)
        {
            gameObject.SetActive(false);
            m_fireProcessHappening = false;
        }
    }

    public void gridUpdate(FireGrassRemover script)
    {
        // combustion will not start until ignition() is called in heatsUp() - called by FireGrid
        // this will ensure that no fire is instantiated and is deleted at the right time
        combustion();

        if (m_extinguish && !m_extingushed)
        {
            // delete fire particle system
            script.deleteGrassOnPosition(transform.position);
            m_extingushed = true;

            if (m_fireBox != null)
            {
                m_fireBox = null;
            }

            delete();
        }
    }

    private void instantiateFire(Vector3 position, GameObject Fire)
    {
        for (int i = 0; i < m_fires.Length; i++)
        {
            m_fires[i] = (GameObject)Instantiate(Fire, position + new Vector3(m_firesPositions[i].x, 0.0f, m_firesPositions[i].y), new Quaternion(), transform);
        }
    }

    public void ingition(Vector3 position, GameObject Fire)
    {
        if (m_fireJustStarted)
        {
            m_isAlight = true;

            for (int i = 0; i < m_fires.Length; i++)
            {
                FireVisualManager visualMgr = m_fires[i].GetComponent<FireVisualManager>();
                visualMgr.setIgnitionState();
            }
        }
    }

    // computes what values should be depending on different factors that affect the behaviour of the fire propagation
    public void setInitialFireValues(float airTemp, float globalFirePropagationSpeed)
    {
        m_ignitionTemperature = (m_HP - airTemp) + m_moisture;

        if (m_HP > 0.0f)
            m_fireTemperature += (m_fuel / m_HP) + globalFirePropagationSpeed;
        else
            m_fireTemperature += globalFirePropagationSpeed;
    }

    public void heatsUp()
    {
        if (m_instantiatedInCell == false)
        {
            instantiateFire(transform.position, m_firePrefab);
            m_instantiatedInCell = true;
            m_fireProcessHappening = true;

            for (int i = 0; i < m_fires.Length; i++)
            {
                FireVisualManager visualMgr = m_fires[i].GetComponent<FireVisualManager>();
                visualMgr.setHeatState();
            }
        }

        if (m_ignitionTemperature > 0.0f)
            m_ignitionTemperature -= m_fireTemperature * Time.deltaTime;

        if (m_ignitionTemperature <= 0.0f && !m_isAlight)
        {
            m_fireJustStarted = true;
            ignition();
        }
    }

    void ignition()
    {
        if (m_fireJustStarted && !m_extingushed)
            ingition(transform.position, m_firePrefab);
    }

    void combustion()
    {
        if (m_isAlight)
        {
            m_fireJustStarted = false;

            m_fuel -= m_combustionConstant * Time.deltaTime;

            if(m_fuel < m_extinguishThreshold)
            {
                for (int i = 0; i < m_fires.Length; i++)
                {
                    FireVisualManager visualMgr = m_fires[i].GetComponent<FireVisualManager>();
                    visualMgr.setExtingushState();
                }
            }

            if (m_fuel <= 0.0f)
            {
                m_isAlight = false;
                m_extinguish = true;
            }

            // is there a collision in this cell with a GameObject with a FireNodeChain
            m_fireBox.detectionTest();
        }
    }
}
