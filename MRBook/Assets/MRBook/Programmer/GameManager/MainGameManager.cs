using UnityEngine;

public class MainGameManager : BaseManager<MainGameManager>
{
    Animator m_Animator;
    PlayButton playButton;

    protected override void Start()
    {
        base.Start();
        m_Animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 再生する
    /// </summary>
    public void Play(PlayButton playButton)
    {
        //イベントのトリガーをチェックしていく
        GameObject[] eventTriggers = GameObject.FindGameObjectsWithTag("Trigger");

        for(int i = 0;i< eventTriggers.Length;i++)
        {
            eventTriggers[i].GetComponent<MyEventTrigger>().SetFlag();
        }

        this.playButton = playButton;
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
}
