using System;
using System.Collections;
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

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(spatialMappingManager.gameObject);
    }

    //このメソッドはAwakeより後でStartよりも前に呼ばれることに注意
    protected override void WasLoaded(Scene sceneName, LoadSceneMode sceneMode)
    {
        Debug.Log("was loaded");
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
                if (titleView != null) titleView.HideAll();

                spatialMappingManager.DrawVisualMeshes = false;
                for (int i = 0; i < spatialMappingManager.transform.childCount; i++)
                {
                    spatialMappingManager.transform.GetChild(i).gameObject.layer = 2;
                }

                //mainSceneManager = MainSceneManager.I;
                //mainSceneManager.GameStart(bookTransform.position, bookTransform.rotation);

                //NotificationManager.I.SetDefaultTransform(bookTransform.position, bookTransform.rotation);

                break;
            case SceneState.Result:
                break;
        }
    }
}