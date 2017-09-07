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
    
    public Transform bookTransform = null;

    Vector3 firstBookPosition = Vector3.zero;
    Quaternion firstBookRotation = Quaternion.identity;

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
                Debug.Log("on loaded main scene");

                if (titleView != null) titleView.HideAll();

                spatialMappingManager.DrawVisualMeshes = false;
                for (int i = 0; i < spatialMappingManager.transform.childCount; i++)
                {
                    spatialMappingManager.transform.GetChild(i).gameObject.layer = 2;
                }

                mainSceneManager = MainSceneManager.I;
                firstBookPosition = bookTransform.position - worldAnchors[0].transform.position;
                firstBookRotation = bookTransform.rotation;
                mainSceneManager.GameStart(bookTransform.position, bookTransform.rotation);

                NotificationManager.I.SetDefaultTransform(bookTransform.position, bookTransform.rotation);

                Debug.Log("book is lock " + bookTransform.position);
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

    public void WorldAnchorsOperation(bool isSave)
    {
        if (worldAnchors == null)
            worldAnchors = worldAnchorContainer.GetComponentsInChildren<WorldAnchorController>();

        for (int i = 0;i< worldAnchors.Length;i++)
        {
            if (isSave)
                worldAnchors[i].SaveAnchor();
            else
                worldAnchors[i].DeleteAnchor();
        }
    }

    public void ModifiBookPosition()
    {
        if (currentSceneState != SceneState.Main) return;
        mainSceneManager.SetBookPositionByAnchor(worldAnchors[0].transform.position + firstBookPosition, bookTransform.rotation);
        NotificationManager.I.ShowDialog("警告", "ホログラムのずれを検知しました。", true, 1.0f);
    }
}