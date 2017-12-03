using System;
using System.Collections;
using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;
using KKUtilities;

public class MainSceneManager : BaseManager<MainSceneManager>
{
    public enum GameState
    {
        Wait,   //再生待機中
        Play,   //再生中
        Next,   //ページが捲られるまで待機
        Back    //ページをさかのぼっている状態
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
    public Action OnReset;
    /// <summary>
    /// 再生が終了したとき
    /// </summary>
    public Action<bool> OnPlayEnd;
    /// <summary>
    /// ステートが変化したとき
    /// </summary>
    public Action<GameState> OnGameStateChanged;

    /* メンバ */
    GameState currentState = GameState.Wait;
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

    public Animator m_Animator;

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

    HoloObjResetManager resetManager;

    /* メソッド */

    protected override void Awake()
    {
        base.Awake();
        pageIndex = -1;
        m_Animator = GetComponent<Animator>();
        
        SpatialMappingManager spatialMappingManager = SpatialMappingManager.Instance;

        if (spatialMappingManager != null)
        {
            spatialMappingManager.DrawVisualMeshes = false;
            for (int i = 0; i < spatialMappingManager.transform.childCount; i++)
            {
                spatialMappingManager.transform.GetChild(i).gameObject.layer = 2;
            }
        }

        ActorManager.I.InitSceneManager(this);
    }

    protected override void Start()
    {
        base.Start();
        OnGameStart += GameStart;

        //todo:UIで読み込み中と出す

        //この時点ではBookTransfromが正しい位置にいないので遅らせる
        Utilities.Delay(1.0f, () =>
        {
            OnGameStart.Invoke();
        }, this);
        resetManager = new HoloObjResetManager(this);
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
        
        if (OnPlayPage != null) OnPlayPage(pages[currentPageIndex]);

        m_Animator.SetBool("IsStart", true);

        pages[currentPageIndex].PlayPage();
    }

    /// <summary>
    /// 再生が終了した
    /// </summary>
    public virtual void EndCallBack(bool success)
    {
        if (OnPlayEnd != null) OnPlayEnd.Invoke(success);
        if (success)
        {
            CurrentState = GameState.Next;
            AkSoundEngine.PostEvent("Clear_" + (currentPageIndex + 1) + "p", gameObject);
        }
        else
        {
            CurrentState = GameState.Wait;
            MainGameUIController.I.endingManager.Show();

            //todo:ここじゃなくて死んだ瞬間に鳴らす
            GameObject urashima = ActorManager.I.GetCharacter(ActorName.Urashima).gameObject;
            AkSoundEngine.PostEvent("Die", urashima);
            AkSoundEngine.PostEvent("Mistake_" + (currentPageIndex + 1) + "p", gameObject);
        }
        CurrentState = GameState.Next;

        if (!success)
        {
            //Utilities.Delay(0.2f, () => ResetPage(), this);
        }

        m_Animator.SetBool("IsStart", false);
    }
    
    public virtual void GameStart()
    {
        Transform t = BookPositionModifier.I.bookTransform;
        
        for(int i = 0;i< pages.Length;i++)
        {
            pages[i].PageLock(t.position, t.rotation);
        }
        MainGameUIController.I.SetPositionAndRotation(t.position, t.rotation);
        NotificationManager.I.SetDefaultTransform(t.position, t.rotation);

        SetPage(currentPageIndex);
        IsGameStart = true;
    }

    /// <summary>
    /// アンカーの位置に本を固定する
    /// </summary>
    public void SetBookPositionOffset(Vector3 movement)
    {
        Transform t = BookPositionModifier.I.bookTransform;
        
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetTransform(t);
        }

        MainGameUIController.I.SetPositionAndRotation(t.position, t.rotation);
        NotificationManager.I.SetDefaultTransform(t.position, t.rotation);
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
        //if (CurrentState != GameState.Wait && CurrentState != GameState.Next) return;

        if (OnReset != null) OnReset.Invoke();
        CurrentState = GameState.Wait;
        AkSoundEngine.PostEvent("Reset", gameObject);
        pages[currentPageIndex].ResetPage();
    }

    /// <summary>
    /// 指定されたページへ遷移する(実際のページの遷移はここ)
    /// </summary>
    /// <param name="isBack">前のページか？</param>
    protected virtual void SetPage(int index)
    {
        //前のページは消す
        pages[currentPageIndex].gameObject.SetActive(false);

        //ページを切り替える
        if (OnPageChanged != null) OnPageChanged.Invoke(pages[currentPageIndex], pages[index]);
        currentPageIndex = index;

        //表示するtodo:エフェクト
        pages[currentPageIndex].gameObject.SetActive(true);
        pages[currentPageIndex].PageStart();
        m_Animator.runtimeAnimatorController = pages[currentPageIndex].controller;

        //todo:UIControllerで行うべき
        //if (isFirst)
        //{
        //    pageIndex = currentPageIndex;
        //    currentMissionText = pages[currentPageIndex].missionText;
        //}
        CurrentState = GameState.Wait;
    }
}
