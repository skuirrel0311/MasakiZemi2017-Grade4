using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : BaseManager<TestSceneManager>
{
    Animator m_Animator;

    protected override void Start()
    {
        base.Start();
        m_Animator = GetComponent<Animator>();
        Play();
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
