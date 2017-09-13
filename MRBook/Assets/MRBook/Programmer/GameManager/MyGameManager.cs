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

    //部屋に配置するWorldAnchor
    [SerializeField]
    Transform worldAnchorContainer = null;
    WorldAnchorController[] worldAnchors;

    public Transform bookTransform = null;

    Vector3 firstBookPosition = Vector3.zero;
    Vector3 firstBookRotationVec = Vector3.zero;

    Vector3 oldWorldAnchorPosition;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(spatialMappingManager.gameObject);

        if (worldAnchors == null)
            worldAnchors = worldAnchorContainer.GetComponentsInChildren<WorldAnchorController>();
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(MonitorWorldAnchor());
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
                
                firstBookPosition = bookTransform.position - worldAnchors[0].transform.position;
                firstBookRotationVec = bookTransform.rotation.eulerAngles - worldAnchors[0].transform.eulerAngles;

                mainSceneManager = MainSceneManager.I;
                mainSceneManager.GameStart(bookTransform.position, bookTransform.rotation);

                NotificationManager.I.SetDefaultTransform(bookTransform.position, bookTransform.rotation);

                break;
            case SceneState.Result:
                break;
        }
    }

    IEnumerator MonitorWorldAnchor()
    {
        WaitForSeconds wait = new WaitForSeconds(1.0f);
        oldWorldAnchorPosition = worldAnchors[0].transform.position;

        while (true)
        {
            yield return wait;

            if (IsChangeWorldAnchorPosition())
            {
                ModifyBookPosition(true);
            }
            oldWorldAnchorPosition = worldAnchors[0].transform.position;
        }
    }

    bool IsChangeWorldAnchorPosition()
    {
        float temp = 0.2f;

        Vector3 difVec = oldWorldAnchorPosition - worldAnchors[0].transform.position;
        if (Mathf.Abs(difVec.x) > temp) return true;
        if (Mathf.Abs(difVec.y) > temp) return true;
        if (Mathf.Abs(difVec.z) > temp) return true;

        return false;
    }

    public void SetWorldAnchorsRendererActive(bool isActive)
    {
        for (int i = 0; i < worldAnchors.Length; i++)
        {
            worldAnchors[i].SetActive(isActive);
        }
    }

    public void WorldAnchorsOperation(bool isSave)
    {
        for (int i = 0; i < worldAnchors.Length; i++)
        {
            if (isSave)
                worldAnchors[i].SaveAnchor();
            else
                worldAnchors[i].DeleteAnchor();
        }
    }

    public void ModifyBookPosition(bool showDialog)
    {
        if (currentSceneState != SceneState.Main) return;
        Debug.Log("modify");
        mainSceneManager.SetBookPositionByAnchor(bookTransform.position, bookTransform.rotation);
        if(showDialog) NotificationManager.I.ShowDialog("警告", "ホログラムのずれを検知しました。", true, 3.0f);
    }
}