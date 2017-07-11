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

    MainGameManager.GameState oldGameState;

    protected override void Start()
    {
        base.Start();
        gameManager = MainGameManager.I;
        oldGameState = gameManager.currentState;
    }

    public void SetPositionAndRotation(Transform anchorTransform)
    {
        transform.SetPositionAndRotation(anchorTransform.position, anchorTransform.rotation);

        missionText.gameObject.SetActive(true);
        stateText.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        resetButton.gameObject.SetActive(true);
    }

    void Update()
    {
        if (oldGameState != gameManager.currentState)
        {
            OnGameStateChanged();
        }

        oldGameState = gameManager.currentState;
    }

    void OnGameStateChanged()
    {
        stateText.CurrentText = gameManager.currentState + ":" + gameManager.currentPageIndex;
        switch (gameManager.currentState)
        {
            case MainGameManager.GameState.Play:
                playButton.Hide();
                resetButton.Hide();
                break;
            case MainGameManager.GameState.Next:
                missionText.CurrentText = "ページをめくれ！";
                playButton.Hide();
                resetButton.Hide();
                break;
            case MainGameManager.GameState.Wait:
                missionText.CurrentText = gameManager.currentMissionText;
                playButton.Refresh();
                resetButton.Refresh();
                break;
        }

    }
}
