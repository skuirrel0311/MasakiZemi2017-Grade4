using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : BaseManager<ResultManager>
{
    [SerializeField]
    HoloText totalScore = null;
    [SerializeField]
    HoloButton titleBack = null;
    [SerializeField]
    LifePointGauge lifePointGauge = null;

    [SerializeField]
    GameObject gameover = null;

    //玉手箱の中身
    List<HoloItem> secretBoxContentsList = new List<HoloItem>();

    public Action onGameOver;
    public bool isGameOver = false;

    public int deathCount = 0;

    protected override void Start()
    {
        base.Start();

        MainSceneManager sceneManager = MainSceneManager.I;

        int maxLifePoint = 0;

        for (int i = 0; i < sceneManager.pages.Length; i++)
        {
            if (maxLifePoint >= sceneManager.pages[i].lifePoint) continue;
            maxLifePoint = sceneManager.pages[i].lifePoint;
        }

        lifePointGauge.Init(maxLifePoint);

        sceneManager.OnPageLoaded += (page) =>
        {
            lifePointGauge.SetValue(page.lifePoint);
        };
    }

    public void AddSecretBoxContent(HoloItem item)
    {
        secretBoxContentsList.Add(item);
    }

    public void RemoveSecretBoxContent(HoloItem item)
    {
        secretBoxContentsList.Remove(item);
    }

    public void RemoveAllSecretBoxContents()
    {
        for (int i = 0; i < secretBoxContentsList.Count; i++)
        {
            secretBoxContentsList[i].owner = null;
        }
        secretBoxContentsList.Clear();
    }

    public int GetContentKind()
    {
        if (secretBoxContentsList.Count == 1) return 1;
        return 0;
    }

    public void AddDeathCount()
    {
        deathCount++;

        lifePointGauge.SetValue(lifePointGauge.value - 1);

        if (lifePointGauge.value <= 0)
        {
            //ゲームオーバー
            if (onGameOver != null) onGameOver.Invoke();
            gameover.gameObject.SetActive(true);
            MyObjControllerByBoundingBox.I.canClick = false;
            isGameOver = true;
            ShowTitleBack();
        }
    }

    public void ShowTitleBack()
    {
        titleBack.gameObject.SetActive(true);
    }

    public void TitleBack()
    {
        SceneManager.LoadScene("Title");
    }

    public void ShowTotalResult()
    {
        totalScore.CurrentText = "死んだ回数：" + deathCount + " 回";

        totalScore.gameObject.SetActive(true);
    }
}
