using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;

/// <summary>
/// タイトルで設定された時の位置からずれた時に自動で直してくれる人
/// </summary>
public class BookPositionModifier : BaseManager<BookPositionModifier>
{
    [SerializeField]
    Transform bookTransform = null;

    //部屋に配置するWorldAnchor
    [SerializeField]
    Transform worldAnchorContainer = null;

    [SerializeField]
    float positionCheckInterval = 1.0f;

    MyGameManager gameManager;
    MainSceneManager mainSceneManager;

    WorldAnchorController[] worldAnchors;

    Vector3 oldWorldAnchorPosition;

    protected override void Awake()
    {
        base.Awake();

        if (worldAnchors != null) return;

        worldAnchors = worldAnchorContainer.GetComponentsInChildren<WorldAnchorController>();
    }

    protected override void Start()
    {
        base.Start();

        gameManager = MyGameManager.I;
        mainSceneManager = MainSceneManager.I;

        WorldManager.OnPositionalLocatorStateChanged += WorldManagerOnStateChanged;
    }

    IEnumerator MonitorWorldAnchor()
    {
        WaitForSeconds wait = new WaitForSeconds(positionCheckInterval);

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
        float temp = 0.3f;

        Vector3 difVec = oldWorldAnchorPosition - worldAnchors[0].transform.position;

        if (Mathf.Abs(difVec.x) > temp) return true;
        if (Mathf.Abs(difVec.y) > temp) return true;
        if (Mathf.Abs(difVec.z) > temp) return true;

        return false;
    }

    void WorldManagerOnStateChanged(PositionalLocatorState oldState, PositionalLocatorState newState)
    {
        //アクティブに戻った時になおす
        if (newState == PositionalLocatorState.Active)
        {
            ModifyBookPosition(true);
        }
    }

    public void ModifyBookPosition(bool showDialog)
    {
        if (gameManager.currentSceneState != MyGameManager.SceneState.Main) return;
        MainSceneManager.I.SetBookPositionByAnchor(bookTransform.position, bookTransform.rotation);
        if (showDialog) NotificationManager.I.ShowDialog("警告", "ホログラムのずれを検知しました。", true, 3.0f);
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
}
