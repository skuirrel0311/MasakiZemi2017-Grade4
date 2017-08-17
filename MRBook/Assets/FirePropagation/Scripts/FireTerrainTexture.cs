﻿/* Copyright (c) 2016-2017 Lewis Ward
// Fire Propagation System
// author: Lewis Ward
// date  : 11/02/2017
*/
using UnityEngine;
using System.Collections;

[System.Serializable]
public class FireTerrainTexture {
    [Tooltip("Which Terrain Texture does this data map to, starts at 0.")]
    public int m_textureID;
    [Tooltip("Is this a flammable fuel?")]
    public bool m_flammable;
    [Tooltip("Is this the burnt ground texture to replace terrain textures where a fire has been?")]
    public bool m_isGroundBurnTexture = false;
    [Tooltip("Lower value, random fuel value generated between lower and higher value.")]
    public float m_fuelLowerValue = 1.0f;
    [Tooltip("Higher value, random fuel value generated between lower and higher value.")]
    public float m_fuelHigherValue = 2.0f;
    [Tooltip("Lower value, random hit point (used up in heat up step) value generated between lower and higher value.")]
    public float m_HPLowerValue = 1.0f;
    [Tooltip("Higher value, random hit point (used up in heat up step) value generated between lower and higher value.")]
    public float m_HPHigherValue = 2.0f;
    [Tooltip("Amount of ground moisture on this fuel.")]
    public float m_moisture = 0.0f;
    [Tooltip("Higher the value the quicker fire reaches ignition temperature. Fire will propagate faster the higher the value.")]
    public float m_firePropagationSpeed = 20.0f;
    public float propagationSpeed { get { return m_firePropagationSpeed; } }

    public FireTerrainTexture(int ID, bool flammable, bool scorchTexture)
    {
        m_textureID = ID;
        m_flammable = flammable;
        m_isGroundBurnTexture = scorchTexture;

        // if the texture fuel type cannot burn set value to 0, otherwise create values
        if (m_flammable == false)
        {
            m_firePropagationSpeed = 0.0f;
            m_fuelLowerValue = 0.0f;
            m_fuelHigherValue = 0.0f;
            m_HPLowerValue = 0.0f;
            m_HPHigherValue = 0.0f;
        }
    }

    public float cellHP()
    {
        return Random.Range(m_HPLowerValue, m_HPHigherValue);
    }

    public float cellFuel()
    {
        return Random.Range(m_fuelLowerValue, m_fuelHigherValue);
    }

    public float cellMoisture()
    {
        return m_moisture;
    }
}
