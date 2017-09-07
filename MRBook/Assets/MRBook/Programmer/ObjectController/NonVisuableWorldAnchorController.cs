using UnityEngine.VR.WSA;

public class NonVisuableWorldAnchorController : WorldAnchorController
{
    protected override void Start()
    {
        worldAnchorManager = MyWorldAnchorManager.I;

        worldAnchorManager.AddOnLoadedAction((store) =>
        {
            isLoaded = true;
            SaveAnchor();
        });

        m_collider.enabled = false;
    }
    public override void SaveAnchor()
    {
        if (!isLoaded) return;

        worldAnchorManager.SaveAnchor(gameObject);
    }

    public override void DeleteAnchor()
    {
        if (!isLoaded) return;

        worldAnchorManager.anchorStore.Delete(name);
        DestroyImmediate(GetComponent<WorldAnchor>());
    }

    public override void SetActive(bool isActive)
    {

    }
}
