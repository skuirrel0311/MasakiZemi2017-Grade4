using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// こいつの子にUIを配置していく
/// </summary>
public class MainGameUIController : BaseManager<MainGameUIController>
{
    MainGameManager gameManager;

    public HoloText missionText = null;
    public HoloText stateText = null;
    public HoloButton playButton = null;
    public HoloButton resetButton = null;

    protected override void Awake()
    {
        base.Awake();
        missionText.gameObject.SetActive(false);
        stateText.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        gameManager = MainGameManager.I;
        gameManager.OnGameStateChanged += OnGameStateChanged;
        gameManager.OnGameStart += OnGameStart;

#if UNITY_EDITOR
        missionText.gameObject.SetActive(true);
        missionText.CurrentText = "push S key!";
#endif
    }

    public void SetPositionAndRotation(Transform anchorTransform)
    {
        transform.SetPositionAndRotation(anchorTransform.position, anchorTransform.rotation);

        missionText.gameObject.SetActive(true);
        stateText.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        resetButton.gameObject.SetActive(true);
    }

    void OnGameStart()
    {
#if UNITY_EDITOR
        //必要なUIだけアクティブをtrueにする
        playButton.gameObject.SetActive(true);
        resetButton.gameObject.SetActive(true);
        missionText.gameObject.SetActive(true);
        //stateText.gameObject.SetActive(true);
#endif
    }
    void OnGameStateChanged(MainGameManager.GameState currentState)
    {
        stateText.CurrentText = currentState + ":" + gameManager.currentPageIndex;
        switch (currentState)
        {
            case MainGameManager.GameState.Play:
                playButton.Hide();
                resetButton.Hide();
                missionText.gameObject.SetActive(false);
                break;
            case MainGameManager.GameState.Next:
                missionText.gameObject.SetActive(true);
                missionText.CurrentText = "ページをめくれ！";
                playButton.Hide();
                resetButton.Hide();
                break;
            case MainGameManager.GameState.Wait:
                missionText.gameObject.SetActive(true);
                missionText.CurrentText = gameManager.currentMissionText;
                playButton.Refresh();
                resetButton.Refresh();
                break;
        }
    }
}
