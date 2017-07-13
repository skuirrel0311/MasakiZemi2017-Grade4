using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetStoreManager : BaseManager<AssetStoreManager>
{
    public MyAssetStore<Material> materialStore = new MyAssetStore<Material>("Materials/");
    public MyAssetStore<GameObject> particleStore = new MyAssetStore<GameObject>("Particles/");
}
