using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : BaseManager<TestSceneManager>
{
    Animator m_Animator;
    [SerializeField]
    BasePage[] pages = null;
    int currentPageIndex = 0;

    public BasePage currentPage { get; private set; }

    List<GameObject> globalObjectList = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        m_Animator = GetComponent<Animator>();

        for(int i = 0;i< pages.Length;i++)
        {
            pages[i].PageLock(Vector3.zero, Quaternion.identity,i);
        }

        SetPage(currentPageIndex);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            //次のページへ
            pages[currentPageIndex].gameObject.SetActive(false);
            currentPageIndex++;
            if (currentPageIndex >= pages.Length)
            {
                //todo:リザルトへ
                return;
            }
            SetPage(currentPageIndex);
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            //前のページへ
            pages[currentPageIndex].gameObject.SetActive(false);
            currentPageIndex--;
            if(currentPageIndex < 0)
            {
                //そんなに戻れない
                return;
            }
            SetPage(currentPageIndex);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            //再生
            Play();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            //リセット
            pages[currentPageIndex].ResetPage();
        }
    }

    void SetPage(int index)
    {
        currentPageIndex = index;
        ActorManager.I.currentPage = pages[currentPageIndex];
        pages[currentPageIndex].gameObject.SetActive(true);
        pages[currentPageIndex].PageStart();
        m_Animator.runtimeAnimatorController = pages[currentPageIndex].controller;
        NotificationManager.I.ShowMessage(pages[currentPageIndex].missionText);
    }

    /// <summary>
    /// 再生する
    /// </summary>
    public void Play()
    {
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
    public void EndCallBack()
    {
        m_Animator.SetBool("IsStart", false);
    }
}
