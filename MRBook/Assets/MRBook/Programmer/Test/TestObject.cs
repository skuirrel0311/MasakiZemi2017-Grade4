using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class TestObject : MonoBehaviour
{
    [SerializeField]
    GameObject anchor = null;

    MyWorldAnchorManager anchorStoreManager;
    WorldAnchorStore anchorStore;

    private void Start()
    {
        anchorStoreManager = MyWorldAnchorManager.I;

        anchorStore = null;

        StartCoroutine(anchorStoreManager.GetAnchorStore((anchorStore) =>
        {
            this.anchorStore = anchorStore;
            StartUpAnchor();
        }));
    }

    private void OnDestroy()
    {
        anchorStore.Delete(anchor.name);
    }

    void StartUpAnchor()
    {
        anchorStoreManager.AttachingAnchor(anchor);
    }

    public void ShowMessage()
    {
        HoloWindow.I.Show("警告", "メモリが不足しています");
    }
}
