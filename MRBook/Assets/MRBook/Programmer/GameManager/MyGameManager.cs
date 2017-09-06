using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using HoloToolkit.Unity.SpatialMapping;

//ゲーム全体を管理するクラス
public class MyGameManager : BaseManager<MyGameManager>
{
    //.unityファイルと同じ名前
    public enum SceneState { Title, Main, Result }

    public SceneState currentSceneState = SceneState.Title;

    MainSceneManager mainSceneManager;

    [SerializeField]
    SpatialMappingManager spatialMappingManager = null;

    [SerializeField]
    TitleView titleView = null;

    //部屋に配置するWorldAnchor
    [SerializeField]
    Transform worldAnchorContainer = null;
    WorldAnchorController[] worldAnchors;

    [SerializeField]
    Transform bookTransform = null;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(spatialMappingManager.gameObject);
    }

    //このメソッドはAwakeより後でStartよりも前に呼ばれることに注意
    protected override void WasLoaded(Scene sceneName, LoadSceneMode sceneMode)
    {
        base.WasLoaded(sceneName, sceneMode);
        //ベースを先に呼ぶことでこのインスタンスがデストロイされたときに以下のコードが実行されることはないはず

        SceneState state = (SceneState)Enum.Parse(typeof(SceneState), sceneName.name);

        currentSceneState = state;

        switch (state)
        {
            case SceneState.Title:
                spatialMappingManager.DrawVisualMeshes = true;
                spatialMappingManager.PhysicsLayer = 10;
                break;
            case SceneState.Main:
                if (titleView != null) titleView.AllHide();
                SetWorldAnchorsRendererActive(false);

                spatialMappingManager.DrawVisualMeshes = false;
                for (int i = 0; i < spatialMappingManager.transform.childCount; i++)
                {
                    spatialMappingManager.transform.GetChild(i).gameObject.layer = 2;
                }

                SetWorldAnchorsRendererActive(false);

                mainSceneManager = MainSceneManager.I;
                mainSceneManager.GameStart(bookTransform);
                break;
            case SceneState.Result:
                break;
        }
    }

    public void SetWorldAnchorsRendererActive(bool isActive)
    {
        if (worldAnchorContainer == null) return;

        if (worldAnchors == null)
            worldAnchors = worldAnchorContainer.GetComponentsInChildren<WorldAnchorController>();

        for (int i = 0; i < worldAnchors.Length; i++)
        {
            worldAnchors[i].SetActive(isActive);
        }

    }
}