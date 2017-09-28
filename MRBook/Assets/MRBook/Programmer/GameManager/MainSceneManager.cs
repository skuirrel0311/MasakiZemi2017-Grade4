using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MainSceneManager : BaseManager<MainSceneManager>
{
    public enum GameState
    {
        Title,  //ゲーム開始画面
        Wait,   //再生待機中
        Play,   //再生中
        Next    //ページが捲られるまで待機
    }

    /* イベント */
    /// <summary>
    /// ページの遷移時　前のページ、次のページ
    /// </summary>
    public Action<BasePage, BasePage> OnPageChanged;
    /// <summary>
    /// TapToStartが押されたとき
    /// </summary>
    public Action OnGameStart;
    /// <summary>
    /// ページを再生させたとき
    /// </summary>
    public Action<BasePage> OnPlayPage;
    /// <summary>
    /// ゲームが終了したとき(リザルトへ行く手前)
    /// </summary>
    public Action<bool> OnPlayEnd;
    /// <summary>
    /// ステートが変化したとき
    /// </summary>
    public Action<GameState> OnGameStateChanged;
    
    /* メンバ */
    GameState currentState = GameState.Title;
    public GameState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            if (currentState == value) return;

            currentState = value;
            if (OnGameStateChanged != null) OnGameStateChanged.Invoke(value);
        }
    }

    public bool IsGameStart { get; protected set; }

    protected Animator m_Animator;

    public string currentMissionText { get; protected set; }

    public BasePage[] pages = null;

    /// <summary>
    /// 本のポリゴンに動的にアタッチされるマテリアル
    /// </summary>
    public Material visibleMat = null;

    /// <summary>
    /// ゲームの進行度合い
    /// </summary>
    public int pageIndex { get; protected set; }

    /// <summary>
    /// 今いるページ
    /// </summary>
    public int currentPageIndex { get; protected set; }

    /* メソッド */

    protected override void Awake()
    {
        base.Awake();
        pageIndex = -1;
        m_Animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        KKUtilities.Delay(1.0f, () =>
        {
            if(OnGameStart != null) OnGameStart.Invoke();
        }, this);
    }

    /// <summary>
    /// 再生する
    /// </summary>
    public virtual void Play()
    {
        if (CurrentState != GameState.Wait) return;
        
        AkSoundEngine.PostEvent("BGM_" + (currentPageIndex + 1) + "p", gameObject);
        CurrentState = GameState.Play;

        NotificationManager.I.ShowMessage("再生開始");

        //イベントのトリガーをチェックしていく
        GameObject[] eventTriggers = GameObject.FindGameObjectsWithTag("Trigger");

        for (int i = 0; i < eventTriggers.Length; i++)
        {
            MyEventTrigger[] tempArray = eventTriggers[i].GetComponents<MyEventTrigger>();

            for (int j = 0; j < tempArray.Length; j++)
            {
                Debug.Log("set flag in maingame " + tempArray[j].flagName);
                tempArray[j].SetFlag();
            }
        }

        m_Animator.SetBool("IsStart", true);

        pages[currentPageIndex].PlayPage();
        if (OnPlayPage != null) OnPlayPage(pages[currentPageIndex]);
    }

    /// <summary>
    /// 再生が終了した
    /// </summary>
    public virtual void EndCallBack(bool success)
    {
        //todo:ページをクリアしたかを判断する
        CurrentState = success ? GameState.Next : GameState.Wait;

        if (!success)
        {
            KKUtilities.Delay(0.2f, () => ResetPage(), this);
        }

        m_Animator.SetBool("IsStart", false);
    }

    /// <summary>
    /// TapToStartが押された
    /// </summary>
    public virtual void GameStart()
    {
        GameStart(Vector3.zero, Quaternion.identity);
    }

    public virtual void GameStart(Vector3 pos, Quaternion rot)
    {
        OnGameStart += () =>
        {
            BookPositionModifier.I.ModifyBookPosition(false);
            SetPage(currentPageIndex);
            IsGameStart = true;
        };
    }

    /// <summary>
    /// アンカーの位置に本を固定する
    /// </summary>
    public void SetBookPositionByAnchor(Vector3 pos, Quaternion rot)
    {
        StartCoroutine(SetPosition(pos, rot));
    }

    IEnumerator SetPosition(Vector3 pos, Quaternion rot)
    {
        pages[currentPageIndex].SetAllAgentEnabled(false);

        yield return null;

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].PageLock(pos, rot, i);
        }
        yield return null;

        pages[currentPageIndex].SetAllAgentEnabled(true);
        MainGameUIController.I.SetPositionAndRotation(pos, rot);
        NotificationManager.I.SetDefaultTransform(pos, rot);
    }

    /// <summary>
    /// 指定されたページへ遷移する。(事前にcurrentPageIndexを弄られると死ぬ)
    /// </summary>
    public void ChangePage(int pageIndex)
    {
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
        }
        else
        {
            //todo:まだ再生できないページは止める
        }

        SetPage(pageIndex);
    }

    /// <summary>
    /// ページを遷移してきたときの状態へ戻す
    /// </summary>
    public virtual void ResetPage()
    {
        if (CurrentState != GameState.Wait) return;

        AkSoundEngine.PostEvent("Reset", gameObject);
        pages[currentPageIndex].ResetPage();
    }

    /// <summary>
    /// 指定されたページへ遷移する(実際のページの遷移はここ)
    /// </summary>
    /// <param name="isBack">前のページか？</param>
    protected virtual void SetPage(int index)
    {
        bool isFirst = (pageIndex + 1) == index;

        //前のページは消す
        pages[currentPageIndex].gameObject.SetActive(false);

        //ページを切り替える
        if (OnPageChanged != null) OnPageChanged.Invoke(pages[currentPageIndex], pages[index]);
        currentPageIndex = index;

        //表示するtodo:エフェクト
        pages[currentPageIndex].gameObject.SetActive(true);
        pages[currentPageIndex].PageStart(isFirst);
        m_Animator.runtimeAnimatorController = pages[currentPageIndex].controller;

        //ミッションの切り替え
        if (isFirst)
        {
            pageIndex = currentPageIndex;
            currentMissionText = pages[currentPageIndex].missionText;
        }
        CurrentState = GameState.Wait;
    }
}
