/* Copyright (c) 2016-2017 Lewis Ward
// Fire Propagation System
// author: Lewis Ward
// date  : 10/02/2017
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireManager : MonoBehaviour {
    private Terrain m_terrain = null; // terrain should be the parent GameObject of this object
    [Tooltip("The Windzone to be used for the simulation.")]
    public WindZone m_windZone;
    [Tooltip("Size of the index array used to keep track of active fires in a FireGrid, if goes over array will increase.")]
    public int m_preAllocatedFireIndexSize = 65;
    [Tooltip("Air temperature in the scene, the higher the value fuels heat up quicker.")]
    public float m_airTemperature = 0.0f;
    [Tooltip("The amount of ground moisture in the scene, higher the value the slower cells heat up.")]
    public float m_groundMoisture = 0.0f;
    [Tooltip("Size of the cell.")]
    public float m_cellSize = 1.0f;
    [Tooltip("Relative position of fire spawning in the cell (0 -> 1). If left empty default value is 0.5 on X and Y.")]
    public Vector2[] m_cellFireSpawnPositions;
    [Tooltip("How fast fuel is used in the combustion step within FireCell's, higher the value the faster fuels are used up.")]
    public float m_combustionRate = 5.0f;
    [Tooltip("How fast fuel is used in the combustion step with FireNode's, higher the value the faster fuels are used up.")]
    public float m_nodeCombustionRate = 5.0f;
    [Tooltip("Smaller the value the more random/less uniformed the propagation.")]
    public float m_propagationBias = 0.4f;
    [Tooltip("Larger the value the faster propagation is up/down hills")]
    public float m_propagationHillBias = 1.0f;
    [Tooltip("Larger the value the faster fire can propagate into the wind, Windzone force can also affect this.")]
    public float m_propagationWindBias = 0.99f;
    [Tooltip("The largest height distance fire can propagate on terrain. Stops fire propagating up large cliffs.")]
    public float m_maxHillPropagationDistance = 0.6f;
    [Tooltip("At what point the extinguish particle systems should be active, is a percentage (0 -> 1). Doesn't affect simulation, only visuals.")]
    public float m_visualExtinguishThreshold = 0.2f;
    [Tooltip("True for day.")]
    public bool m_dayTime = true;
    [Tooltip("The textured used on the terrain are fuels, can set fuel values for each texture.")]
    public FireTerrainTexture[] m_terrainTextures;
    [Tooltip("The index of the terrain grass texture to replace grass with once burnt, this is used only if 'Remove Grass Once Burnt' is disabled.")]
    public int m_burntGrassTextureIndex = 0;
    [Tooltip("Enable detailed fire propagation simulation, this may have a small impact on performance!")]
    public bool m_detailedSimulation = false;
    [Tooltip("Removes grass in a lit fire cell.")]
    public bool m_removeGrassOnceBurnt = false;
    [Tooltip("Replaced ground textures with scorch marks, this may have a small impact on performance!")]
    public bool m_replaceGroundTextureOnceBurnt = true;
    [Tooltip("The about of time taken before the next terrain update.")]
    public float m_terrainUpdateTime = 1.0f;
    [Tooltip("Fire Grid's are constructed over several frames, good when using larger sized grids.")]
    public bool m_staggeredGridConstruction = true;
    [Tooltip("Use as many terrain grass texture details as possible, this may have an impact on performance!")]
    public bool m_maxGrassDetails = false;
    [Tooltip("Which index of the grass detail texture is the burnt grass, only used if 'Max Grass Details' is enabled.")]
    public int m_burntGrassDetailIndex = 1;
    private float m_terrainUpdateTimer = 0.0f; // update timer
    private List<int[,]> m_terrainMaps;
    private List<int[,]> m_terrainMapsOriginal;
    private int[,] m_terrainMap; // used for grass and removing grass (normal)
    private int[,] m_terrainMapOriginal; // used for grass and removing grass (normal)
    private int[,] m_terrainReplaceMap; // used for grass and removing grass (normal)
    private int[,] m_terrainReplaceMapOriginal; // used for grass and removing grass (normal)
    private float[,,] m_terrainTexture; // used for replacing terrain textures
    private float[,,] m_terrainTextureOriginal; // used for replacing terrain textures
    private float m_terrainDetailSize;
    private int m_terrainDetailWidth;
    private int m_terrainDetailHeight;
    private int m_terrainAlphaWidth;
    private int m_terrainAlphaHeight;
    private bool m_dirty = false;
    private int m_activeFireGrids = 0;

    public WindZone windzone 
    {
        get { return m_windZone;  }
        set { m_windZone = value;  }
    }
    public int preAllocatedFireIndexSize
    {
        get { return m_preAllocatedFireIndexSize; }
        set { m_preAllocatedFireIndexSize = value; }
    }
    public float airTemperature
    {
        get { return m_airTemperature; }
        set { m_airTemperature = value; }
    }
    public float groundMoisture
    {
        get { return m_groundMoisture; }
        set { m_groundMoisture = value; }
    }
    public float cellSize
    {
        get { return m_cellSize; }
        set { m_cellSize = value; }
    }
    public Terrain terrain
    {
        get { return m_terrain; }
    }
    public List<int[,]> terrainMaps
    {
        get { return m_terrainMaps; }
        set { m_terrainMaps = value; }
    }
    public int[,] terrainMap
    {
        get { return m_terrainMap; }
        set { m_terrainMap = value; }
    }
    public int[,] terrainReplacementMap
    {
        get { return m_terrainReplaceMap; }
        set { m_terrainReplaceMap = value; }
    }
    public float[,,] terrainAlpha
    {
        get { return m_terrainTexture; }
        set { m_terrainTexture = value; }
    }
    public float terrainDetailSize
    {
        get { return m_terrainDetailSize; }
    }
    public int terrainWidth
    {
        get { return m_terrainDetailWidth; }
    }
    public int terrainHeight
    {
        get { return m_terrainDetailHeight; }
    }
    public int alphaWidth
    {
        get { return m_terrainAlphaWidth; }
    }
    public int alphaHeight
    {
        get { return m_terrainAlphaHeight; }
    }
    public FireTerrainTexture[] terrainTextures
    {
        get { return m_terrainTextures; }
    }
    public float propagationBias
    {
        get { return m_propagationBias; }
        set { m_propagationBias = value; }
    }
    public float propagationWindBias
    {
        get { return m_propagationWindBias; }
        set { m_propagationWindBias = value; }
    }
    public float propagationHillBias
    {
        get { return m_propagationHillBias; }
        set { m_propagationHillBias = value; }
    }
    public float maxHillPropagationDistance
    {
        get { return m_maxHillPropagationDistance; }
        set { m_maxHillPropagationDistance = value; }
    }
    public float visualExtinguishThreshold
    {
        get { return m_visualExtinguishThreshold; }
        set { m_visualExtinguishThreshold = value; }
    }
    public float combustionRate
    {
        get { return m_combustionRate; }
    }
    public float nodeCombustionRate
    {
        get { return m_nodeCombustionRate; }
    }
    public bool daytime
    {
        get { return m_dayTime; }
        set { m_dayTime = value; }
    }
    public bool dirty
    {
        get { return m_dirty; }
        set { m_dirty = value; }
    }
    public bool detailedSimulation
    {
        get { return m_detailedSimulation; }
    }
    public Vector2[] cellFireSpawnPositions
    {
        get { return m_cellFireSpawnPositions; }
    }
    public bool staggeredGridConstruction
    {
        get { return m_staggeredGridConstruction; }
    }

    void Awake()
    {
        // cannot be smaller then cell size otherwise fire will not propagate
        if (m_maxHillPropagationDistance < m_cellSize)
            m_maxHillPropagationDistance = m_cellSize;

        // make sure within 0->1 range 
        if (m_visualExtinguishThreshold > 1.0f)
            m_visualExtinguishThreshold = 1.0f;
        if (m_visualExtinguishThreshold < 0.0f)
            m_visualExtinguishThreshold = 0.0f;

        if (m_combustionRate < 1.0f)
            m_combustionRate = 1.0f;

        if (m_propagationBias < 0.0000001f)
        {
            m_propagationBias = 0.0000001f;
            Debug.Log("Capping propagationBias to 0.0000001f, as it's to smaller or zero");
        }

        if (m_propagationBias > 1.0f)
        {
            m_propagationBias = 1.0f;
            Debug.Log("Capping propagationBias to 1.0f, as it's too large");
        }

        if (m_propagationHillBias < 1.0f)
        {
            m_propagationHillBias = 1.0f;
            Debug.Log("Capping propagationHillBias to 1.0f, as it's too small");
        }

        // get the terrain, need to be a child of a Terrain GameObject
        m_terrain = GetComponentInParent<Terrain>();

        if (m_terrain != null)
        {
            m_terrainDetailWidth = m_terrain.terrainData.detailWidth;
            m_terrainDetailHeight = m_terrain.terrainData.detailHeight;
            m_terrainAlphaWidth = m_terrain.terrainData.alphamapWidth;
            m_terrainAlphaHeight = m_terrain.terrainData.alphamapHeight;

            // use the arrays or the lists
            if (!m_maxGrassDetails)
            {
                m_terrainMap = m_terrain.terrainData.GetDetailLayer(0, 0, m_terrainDetailWidth, m_terrainDetailHeight, 0);
                m_terrainReplaceMap = m_terrain.terrainData.GetDetailLayer(0, 0, m_terrainDetailWidth, m_terrainDetailHeight, 1);
                m_terrainMapOriginal = (int[,])m_terrainMap.Clone(); // performs a deep copy, Clone by itself performs a shallow copy (i.e. 2nd array has references to the 1st array)
                m_terrainReplaceMapOriginal = (int[,])m_terrainReplaceMap.Clone();
            }
            else
            {
                // make sure a valid index was set
                if (m_burntGrassDetailIndex >= m_terrain.terrainData.detailPrototypes.Length || m_burntGrassDetailIndex < 0)
                {
                    m_burntGrassDetailIndex = 0;
                    Debug.Log("Burnt Grass Texture Index is higher/lower then the number of grass texture details set, setting to 0");
                }

                // set up Lists
                m_terrainMaps = new List<int[,]>();
                m_terrainMapsOriginal = new List<int[,]>();
                for (int i = 0; i < m_terrain.terrainData.detailPrototypes.Length; i++)
                {
                    m_terrainMaps.Add(m_terrain.terrainData.GetDetailLayer(0, 0, m_terrainDetailWidth, m_terrainDetailHeight, i));
                    m_terrainMapsOriginal.Add(m_terrain.terrainData.GetDetailLayer(0, 0, m_terrainDetailWidth, m_terrainDetailHeight, i));
                }
            }

            // get the terrain textures
            m_terrainTexture = m_terrain.terrainData.GetAlphamaps(0, 0, m_terrainAlphaWidth, m_terrainAlphaHeight);
            m_terrainTextureOriginal = (float[,,])m_terrainTexture.Clone();


            int TerrainDetailMapSize = m_terrain.terrainData.detailResolution;
            if (m_terrain.terrainData.size.x != m_terrain.terrainData.size.z)
            {
                Debug.Log("X and Y size of terrain have to be the same.");
                return;
            }

            // need to have at least one terrain texture defined
            if (terrainTextures.Length != terrainAlpha.GetLength(2))
            {
                Debug.LogError("A different number of Terrain Textures are set in Fire Manager compared with the Terrain.");
            }

            m_terrainDetailSize = TerrainDetailMapSize / m_terrain.terrainData.size.x;

            if (m_cellFireSpawnPositions.Length == 0)
                m_cellFireSpawnPositions = new Vector2[1] { new Vector2(0.5f, 0.5f) };
        }
        else
        {
            Debug.LogError("Terrain not found! A Fire Manager should be a child of a Terrain GameObject.");
        }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update ()
    {
        // make sure there is a FireGrid somewhere in the world
        if (m_activeFireGrids > 0)
        {
            // start now, so when the first fire goes out the terrain will be instantly updated
            m_terrainUpdateTimer += Time.deltaTime;

            // if dirty, update the terrain. This is set when terrain data is changed like grass being removed by FireGrassRemover
            // also make sure one of the features has been turned one
            if (m_dirty)
            {
                // make sure the set amount of time has past, then call coroutine
                if (m_terrainUpdateTimer >= m_terrainUpdateTime)
                {
                    StartCoroutine(coTerrainUpdate());
                    m_terrainUpdateTimer = 0.0f;
                }
            }
        }
	}

    public void addActiveFireGrid()
    {
        m_activeFireGrids++;
    }

    public void removeActiveFireGrid()
    {
        m_activeFireGrids--;
    }

    void OnApplicationQuit()
    {
        if (m_terrain != null)
        {
            if (!m_maxGrassDetails)
            {
                m_terrain.terrainData.SetDetailLayer(0, 0, 0, m_terrainMapOriginal);
                m_terrain.terrainData.SetDetailLayer(0, 0, 1, m_terrainReplaceMapOriginal);    
            }
            else
            {
                for (int i = 0; i < m_terrain.terrainData.detailPrototypes.Length; i++)
                    m_terrain.terrainData.SetDetailLayer(0, 0, i, m_terrainMapsOriginal[i]);
            }

            m_terrain.terrainData.SetAlphamaps(0, 0, m_terrainTextureOriginal);
            Debug.Log("Restoring map original data");
        }
    }

    public IEnumerator coTerrainUpdate()
    {
        if (!m_maxGrassDetails)
        {
            if (m_removeGrassOnceBurnt)
            {
                // remove grass
                m_terrain.terrainData.SetDetailLayer(0, 0, 0, terrainMap);
                yield return null;
            }
            else
            {
                // replace grass
                m_terrain.terrainData.SetDetailLayer(0, 0, 0, m_terrainMap);
                yield return null;
                m_terrain.terrainData.SetDetailLayer(0, 0, 1, m_terrainReplaceMap);
                yield return null;
            }

            if (m_replaceGroundTextureOnceBurnt)
            {
                m_terrain.terrainData.SetAlphamaps(0, 0, m_terrainTexture);
                yield return null;
            }
        }
        else
        {
            for (int i = 0; i < m_terrain.terrainData.detailPrototypes.Length; i++)
            {
                if (m_removeGrassOnceBurnt)
                {
                    // remove grass
                    m_terrain.terrainData.SetDetailLayer(0, 0, i, m_terrainMaps[i]);
                    yield return null;
                }
                else
                {
                    // replace grass
                    m_terrain.terrainData.SetDetailLayer(0, 0, i, m_terrainMaps[i]);
                    yield return null;
                    m_terrain.terrainData.SetDetailLayer(0, 0, m_burntGrassDetailIndex, m_terrainMaps[m_burntGrassDetailIndex]);
                    yield return null;
                }
            }

            if (m_replaceGroundTextureOnceBurnt)
            {
                m_terrain.terrainData.SetAlphamaps(0, 0, m_terrainTexture);
                yield return null;
            }
        }

        m_dirty = false;
    }
}
