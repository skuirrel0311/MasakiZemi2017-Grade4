using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    //todo:ページの読み込みが終わったとき＝bodyLockからstaticsに変わったときにするといいかも
    /// <summary>
    /// ページの読み込みが終わったとき
    /// </summary>
    public Action<BasePage> OnPageLoaded;
    /// <summary>
    /// ページを再生させたとき
    /// </summary>
    public Action OnPlayPage;
    /// <summary>
    /// 再生が終了したとき
    /// </summary>
    public Action<bool> OnPlayEnd;
    /// <summary>
    /// ページがリセットされたとき
    /// </summary>
    public Action OnReset;
    
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
        }
    }

    public bool IsGameStart { get; protected set; }

    [NonSerialized]
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

    [SerializeField]
    Transform uiContainer = null;

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
        //todo:UIで読み込み中と出す

        //この時点ではBookTransfromが正しい位置にいないので遅らせる
        Utilities.Delay(1.0f, () =>
        {
            GameStart();
        }, this);
        resetManager = new HoloObjResetManager(this);
    }

    /// <summary>
    /// 再生する
    /// </summary>
    public virtual void Play()
    {
        if (CurrentState != GameState.Wait) return;

        if (currentPageIndex == 4 && FlagManager.I.GetFlag("UrashimaIsMacho", 3))
        {
            Debug.Log("macho bgm");
            AkSoundEngine.PostEvent("BGM_Macho", gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("BGM_" + (currentPageIndex + 1) + "p", gameObject);
        }
        CurrentState = GameState.Play;

        //NotificationManager.I.ShowMessage("再生開始");
        
        if (OnPlayPage != null) OnPlayPage();

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
            
            AkSoundEngine.PostEvent("Mistake_" + (currentPageIndex + 1) + "p", gameObject);
        }

        if (!success)
        {
            //Utilities.Delay(0.2f, () => ResetPage(), this);
        }
    }
    
    public virtual void GameStart()
    {
        Transform t = BookPositionModifier.I.bookTransform;
        
        for(int i = 0;i< pages.Length;i++)
        {
            pages[i].PageLock(t.position, t.rotation);
        }

        uiContainer.SetPositionAndRotation(t.position + uiContainer.transform.position, t.rotation);
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
        
        uiContainer.SetPositionAndRotation(t.position, t.rotation);
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
        PageResultManager.I.Hide();
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
        PageResultManager.I.Hide();
        //ページを切り替える
        currentPageIndex = index;

        //表示するtodo:エフェクト
        pages[currentPageIndex].gameObject.SetActive(true);
        pages[currentPageIndex].PageStart();
        m_Animator.runtimeAnimatorController = pages[currentPageIndex].controller;

        CurrentState = GameState.Wait;

        Utilities.Delay(2, () =>
        {
            Debug.Log("on page loaded");
            MyNavMeshBuilder.CreateNavMesh();
            if (OnPageLoaded != null) OnPageLoaded.Invoke(pages[currentPageIndex]);
        },this);
    }

    public void GameEnd()
    {
        SceneManager.LoadSceneAsync("Title");
    }
}
