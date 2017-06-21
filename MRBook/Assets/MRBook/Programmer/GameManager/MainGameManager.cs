using UnityEngine;

public class MainGameManager : BaseManager<MainGameManager>
{
    Animator m_Animator;

    [SerializeField]
    PlayButton playButton;
    Vector3 playButtonPosition;

    [SerializeField]
    BasePage[] pages = null;

    [SerializeField]
    Transform anchor = null;

    int currentPageIndex = 0;

    protected override void Start()
    {
        base.Start();
        playButtonPosition = playButton.transform.position;
        m_Animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 再生する
    /// </summary>
    public void Play()
    {
        //イベントのトリガーをチェックしていく
        GameObject[] eventTriggers = GameObject.FindGameObjectsWithTag("Trigger");

        for(int i = 0;i< eventTriggers.Length;i++)
        {
            eventTriggers[i].GetComponent<MyEventTrigger>().SetFlag();
        }
        m_Animator.SetBool("IsStart", true);
    }
    
    /// <summary>
    /// 再生が終了した
    /// </summary>
    public void EndCallBack()
    {
        m_Animator.SetBool("IsStart", false);
        //もう一回押せるようにする
        playButton.Initialize();
    }

    public void GameStart()
    {
        //絵本の位置
        Vector3 artBookPosition = anchor.position + new Vector3(0.0f, -0.1f, 0.0f);

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].PageLock(artBookPosition, anchor.rotation, i);
        }

        SetPage(currentPageIndex);

        playButton.transform.position = artBookPosition + playButtonPosition;
        playButton.transform.rotation = anchor.rotation;
        playButton.gameObject.SetActive(true);
    }
    
    public void ChangePage(int pageIndex)
    {
        bool isBack = false;
        if(pageIndex < 0)
        {
            //そんなページはない
            return;
        }

        if(pageIndex >= pages.Length)
        {
            //todo:リザルトへ
            return;
        }

        //移動する必要がない
        if(pageIndex == currentPageIndex) return;

        if(currentPageIndex > pageIndex)
        {
            //もどる場合
            isBack = true;
        }
        else
        {
            //todo:まだ再生できないページは止める
        }

        currentPageIndex = pageIndex;
        SetPage(currentPageIndex, isBack);
    }

    public void ResetPage()
    {
        pages[currentPageIndex].ResetPage();
    }

    void SetPage(int index,bool isBack = false)
    {
        currentPageIndex = index;
        ActorManager.I.currentPage = pages[currentPageIndex];
        pages[currentPageIndex].gameObject.SetActive(true);
        pages[currentPageIndex].PageStart();
        m_Animator.runtimeAnimatorController = pages[currentPageIndex].controller;
        if(!isBack) NotificationManager.I.ShowMessage(pages[currentPageIndex].missionText);
    }
}
