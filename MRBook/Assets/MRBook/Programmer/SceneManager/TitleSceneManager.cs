using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HoloToolkit.Unity.SpatialMapping;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    Transform bookPosition = null;

    [SerializeField]
    Transform bookPositionController = null;

    [SerializeField]
    TitleView titleView = null;

    void Start()
    {

        SpatialMappingManager spatialMappingManager = SpatialMappingManager.Instance;
        if (spatialMappingManager == null) return;
        spatialMappingManager.DrawVisualMeshes = true;
        spatialMappingManager.PhysicsLayer = 10;
    }

    public void GameStart()
    {
        bookPosition.position = bookPositionController.position;
        bookPosition.rotation = bookPositionController.rotation;
        titleView.HideAll();

        Debug.LogWarning("BookPos = " + BookPositionModifier.I.bookTransform.position);
        SceneManager.LoadSceneAsync("main");
    }
}
