using System;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : BaseManager<MainGameManager>
{
    public enum GameState
    {
        Wait,   //再生待機中
        Play,   //再生中
        Next   //ページが捲られるまで待機
    }
    
    /// <summary>
    /// ページの遷移時に呼ばれる　前のページ、次のページ
    /// </summary>
    public event Action<BasePage, BasePage> OnPageChanged;

    public GameState currentState { get; private set; }
    protected GameState oldState = GameState.Wait;

    protected Animator m_Animator;

    protected MainGameUIController uiController;

    public string currentMissionText { get; protected set; }

    public BasePage[] pages = null;

    public Material visibleMat = null;

    /// <summary>
    /// 初期位置を決める用のアンカー
    /// </summary>
    [SerializeField]
    Transform anchor = null;

    /// <summary>
    /// ゲームの進行度合い
    /// </summary>
    public int pageIndex { get; protected set; }

    /// <summary>
    /// 今いるページ
    /// </summary>
    public int currentPageIndex { get; protected set; }

    protected override void Start()
    {
        base.Start();
        m_Animator = GetComponent<Animator>();
        uiController = MainGameUIController.I;
        ActorManager.I.currentPage = pages[0];
    }

    /// <summary>
    /// 再生する
    /// </summary>
    public virtual void Play()
    {
        currentState = GameState.Play;

        NotificationManager.I.ShowMessage("再生開始");

        //イベントのトリガーをチェックしていく
        GameObject[] eventTriggers = GameObject.FindGameObjectsWithTag("Trigger");

        for (int i = 0; i < eventTriggers.Length; i++)
        {
            eventTriggers[i].GetComponent<MyEventTrigger>().SetFlag();
        }
        m_Animator.SetBool("IsStart", true);
    }

    /// <summary>
    /// 再生が終了した
    /// </summary>
    public virtual void EndCallBack(bool success)
    {
        //todo:ページをクリアしたかを判断する
        currentState = success ? GameState.Next : GameState.Wait;

        m_Animator.SetBool("IsStart", false);
    }

    /// <summary>
    /// TapToStartが押された
    /// </summary>
    public virtual void GameStart()
    {
        //絵本の位置
        Vector3 artBookPosition = anchor.position + new Vector3(0.0f, -0.1f, 0.0f);

        for (int i = 0; i < pages.Length; i++)
        {
            Vector3 rotation = anchor.eulerAngles + pages[i].transform.eulerAngles;
            pages[i].PageLock(artBookPosition, Quaternion.Euler(rotation), i);
        }

        SetPage(currentPageIndex);

        uiController.SetPositionAndRotation(anchor);
    }

    /// <summary>
    /// 指定されたページへ遷移する。(事前にcurrentPageIndexを弄られると死ぬ)
    /// </summary>
    public void ChangePage(int pageIndex)
    {
        bool isBack = false;
        if (pageIndex < 0)
        {
            //そんなページはない
            return;
        }

        if (pageIndex >= pages.Length)
        {
            //todo:リザルトへ
            return;
        }

        //移動する必要がない
        if (pageIndex == currentPageIndex) return;

        if (currentPageIndex > pageIndex)
        {
            //もどる場合
            isBack = true;
        }
        else
        {
            //todo:まだ再生できないページは止める
        }

        SetPage(pageIndex, isBack);
    }

    /// <summary>
    /// ページを遷移してきたときの状態へ戻す
    /// </summary>
    public virtual void ResetPage()
    {
        pages[currentPageIndex].ResetPage(() =>
        {
            uiController.resetButton.Refresh();
        });
    }

    /// <summary>
    /// 指定されたページへ遷移する(実際のページの遷移はここ)
    /// </summary>
    /// <param name="isBack">前のページか？</param>
    protected virtual void SetPage(int index, bool isBack = false)
    {
        //前のページは消す
        pages[currentPageIndex].gameObject.SetActive(false);

        //ページを切り替える
        if (OnPageChanged != null) OnPageChanged.Invoke(pages[currentPageIndex], pages[index]);
        currentPageIndex = index;

        //表示するtodo:エフェクト
        pages[currentPageIndex].gameObject.SetActive(true);
        pages[currentPageIndex].PageStart(isBack);
        m_Animator.runtimeAnimatorController = pages[currentPageIndex].controller;

        //ミッションの切り替え
        if (!isBack)
        {
            pageIndex = currentPageIndex;
            currentMissionText = pages[currentPageIndex].missionText;
        }
        currentState = GameState.Wait;
    }
}
