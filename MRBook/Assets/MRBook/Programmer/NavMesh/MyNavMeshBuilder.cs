using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

[DefaultExecutionOrder(-102)]
public class MyNavMeshBuilder : MonoBehaviour
{
    static List<MyNavMeshBuilder> builderList = new List<MyNavMeshBuilder>();

    // The size of the build bounds
    public Vector3 m_Size = new Vector3(0.5f, 0.5f, 0.5f);

    NavMeshData m_NavMesh;
    AsyncOperation m_Operation;
    NavMeshDataInstance m_Instance;
    List<NavMeshBuildSource> m_Sources = new List<NavMeshBuildSource>();
    NavMeshBuildSettings setting;

    void OnEnable()
    {
        // Construct and add navmesh
        m_NavMesh = new NavMeshData();
        m_Instance = NavMesh.AddNavMeshData(m_NavMesh);
        builderList.Add(this);
    }

    void OnDisable()
    {
        // Unload navmesh and clear handle
        m_Instance.Remove();
        builderList.Remove(this);
    }

    void Start()
    {
        setting = NavMesh.GetSettingsByID(0);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UpdateNavMesh();
        }
    }
#endif
    public static void CreateNavMesh()
    {
        for(int i = 0;i< builderList.Count;i++)
        {
            builderList[i].UpdateNavMesh();
        }
    }

    public void UpdateNavMesh()
    {
        NavMeshSourceTag.Collect(ref m_Sources);
        var bounds = QuantizedBounds();
        NavMeshBuilder.UpdateNavMeshData(m_NavMesh, setting, m_Sources, bounds);
    }

    static Vector3 Quantize(Vector3 v)
    {
        const float sizeRate = 0.001f;
        v *= 1000.0f;

        v.x = Mathf.Round(v.x) * sizeRate;
        v.y = Mathf.Round(v.y) * sizeRate;
        v.z = Mathf.Round(v.z) * sizeRate;

        return v;
    }

    Bounds QuantizedBounds()
    {
        // Quantize the bounds to update only when theres a 10% change in size
        var center = transform.position;
        return new Bounds(Quantize(center), m_Size);
    }

    private void OnDrawGizmosSelected()
    {
        Color temp = Gizmos.color;
        Gizmos.color = Color.yellow;
        var bounds = QuantizedBounds();
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.color = temp;
    }
}
