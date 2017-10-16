using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// こいつの子にUIを配置していく
/// </summary>
public class MainGameUIController : BaseManager<MainGameUIController>
{
    MainSceneManager gameManager;

    public HoloText missionText = null;
    public HoloText stateText = null;
    public HoloButton playButton = null;
    public HoloButton resetButton = null;
    public EndingManager endingManager = null;

    protected override void Start()
    {
        base.Start();
        gameManager = MainSceneManager.I;
        gameManager.OnGameStateChanged += OnGameStateChanged;
    }

    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }

    void OnGameStateChanged(MainSceneManager.GameState currentState)
    {
        stateText.CurrentText = currentState + ":" + gameManager.currentPageIndex;
        switch (currentState)
        {
            case MainSceneManager.GameState.Play:
                playButton.Disable();
                resetButton.Disable();
                //missionText.gameObject.SetActive(false);
                break;
            case MainSceneManager.GameState.Next:
                //missionText.gameObject.SetActive(true);
                //missionText.CurrentText = "ページをめくれ！";
                playButton.Disable();
                //resetButton.Disable();
                resetButton.Refresh();
                break;
            case MainSceneManager.GameState.Wait:
                //missionText.gameObject.SetActive(true);
                //missionText.CurrentText = gameManager.currentMissionText;
                endingManager.Hide();
                playButton.Refresh();
                resetButton.Refresh();
                break;
        }
    }
}
