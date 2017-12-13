using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// meshやmaterialはResourcesからロードする
/// </summary>
public class ChangeMesh : BaseStateMachineBehaviour
{
    [SerializeField]
    string meshName = "";
    [SerializeField]
    string materialName = "";

    [SerializeField]
    string objName = "";
    [SerializeField]
    bool isSkinned = true;

    protected override void OnStart()
    {
        base.OnStart();
        
        Mesh mesh = MyAssetStore.I.GetAsset<Mesh>(meshName, "Meshes/");
        if (mesh == null) return;
        Material material = MyAssetStore.I.GetAsset<Material>(materialName, "Materials/");
        if (material == null) return;
        HoloObject obj = ActorManager.I.GetObject(objName);
        if (obj == null) return;
        if(isSkinned)
        {
            SkinnedMeshRenderer rend;
            rend = obj.GetComponentInChildren<SkinnedMeshRenderer>();

            if (rend == null) return;

            rend.sharedMesh = mesh;
            rend.material = material;
        }
        else
        {
            MeshFilter meshFilter;
            meshFilter = obj.GetComponentInChildren<MeshFilter>();

            if (meshFilter == null) return;

            meshFilter.mesh = mesh;
            MeshRenderer rend;
            rend = obj.GetComponentInChildren<MeshRenderer>();

            if (rend == null) return;
            rend.material = material;
        }
    }
}
