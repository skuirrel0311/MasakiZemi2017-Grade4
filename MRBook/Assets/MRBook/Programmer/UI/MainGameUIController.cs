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
    [SerializeField]
    HoloText totalScore = null;
    [SerializeField]
    HoloButton titleBack = null;

    protected override void Start()
    {
        base.Start();
        gameManager = MainSceneManager.I;
    }

    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }

    void OnGameStateChanged(MainSceneManager.GameState currentState)
    {
        Debug.Log("OnGameStateChanged");
        stateText.CurrentText = currentState + ":" + gameManager.currentPageIndex;
        switch (currentState)
        {
            case MainSceneManager.GameState.Play:
                playButton.Disable();
                resetButton.Disable();
                HandIconController.I.Hide();
                break;
            case MainSceneManager.GameState.Next:
                playButton.Disable();
                //resetButton.Disable();
                resetButton.Refresh();
                break;
            case MainSceneManager.GameState.Wait:
                playButton.Refresh();
                resetButton.Refresh();
                break;
        }
    }

    public void ShowTotalResult()
    {
        totalScore.CurrentText = "死んだ回数：" + ResultManager.I.deathCount + " 回";

        totalScore.gameObject.SetActive(true);
    }

    public void ShowTitleBack()
    {
        titleBack.gameObject.SetActive(true);
    }
}
