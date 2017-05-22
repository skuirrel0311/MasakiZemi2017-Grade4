using UnityEngine;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class MainGameManager : BaseManager<MainGameManager>
{
    [SerializeField]
    GameObject popupAnchor = null;

    WorldAnchor anchor;
    WorldAnchorStore anchorStore;

    protected override void Start()
    {
        base.Start();
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    void AnchorStoreReady(WorldAnchorStore anchorStore)
    {
        this.anchorStore = anchorStore;
        SetAnchor();
    }

    void SetAnchor()
    {
        WorldAnchor attached = anchorStore.Load(popupAnchor.name, popupAnchor);

        if (attached == null)
        {
            Debug.Log("ロード失敗");
            bool s = anchorStore.Save(popupAnchor.name, popupAnchor.GetComponent<WorldAnchor>());
            if (!s) Debug.Log("セーブ失敗");
            else popupAnchor.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            Debug.Log("ロード成功");
            popupAnchor.GetComponent<Renderer>().material.color = Color.blue;
        }
        popupAnchor.SetActive(true);
    }
}
